using Microsoft.Windows.AppLifecycle;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Windows.ApplicationModel.Activation;
using Windows.System;


namespace WebAuthenticator_Sample
{
    /// https://learn.microsoft.com/en-us/windows/uwp/launch-resume/handle-uri-activation
    /// <summary>
    /// Handles Authentication redirection to the system browser and re-activation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Your app must be configured for Authentication. In you app package's <c>Package.appxmanifest</c> under Declarations, add a 
    /// Protocol declaration and add the scheme you registered for your application's authentication redirect url under "Name".
    /// </para>
    /// </remarks>
    public sealed class WinWebAuthenticator
    {
        /// <summary>
        /// Begin an authentication flow by navigating to the specified url and waiting for a callback/redirect to the callbackUrl scheme.
        /// </summary>
        /// <param name="authorizeUri">Url to navigate to, beginning the authentication flow.</param>
        /// <param name="callbackUri">Expected callback url that the navigation flow will eventually redirect to.</param>
        /// <returns>Returns a result parsed out from the callback url.</returns>
        /// <remarks>Prior to calling this, a call to <see cref="CheckAuthenticationRedirectionActivation(bool)"/> must be made during application startup.</remarks>
        /// <seealso cref="CheckAuthenticationRedirectionActivation(bool)"/>
        public static Task<WebAuthenticatorResult> AuthenticateAsync(Uri authorizeUri, Uri callbackUri) => _instance.Authenticate(authorizeUri, callbackUri, CancellationToken.None);

        private static  bool _authencationCheckWasPerformed;
        private TaskCompletionSource<WebAuthenticatorResult> _tcsResponse = null; 
        private Uri _currentRedirectUri = null;

        private static readonly WinWebAuthenticator _instance = new WinWebAuthenticator();

        static WinWebAuthenticator()
        {
        }

        private WinWebAuthenticator()
        {
            AppInstance.GetCurrent().Activated += CurrentAppInstance_Activated;
        }

        /// <summary>
        /// <Extensions>
        ///  <uap:Extension Category = "windows.protocol" >
        ///    < uap:Protocol Name = "com.companyname.webauthenticator.sample" >
        ///      < uap:DisplayName>WebAuthenticator_Sample</uap:DisplayName>
        ///    </uap:Protocol>
        ///  </uap:Extension>
        ///</Extensions>
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        private static bool IsUriProtocolDeclared(string scheme)
        {
            if (global::Windows.ApplicationModel.Package.Current is null)
                return false;
            var docPath = Path.Combine(global::Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "AppxManifest.xml");
            var doc = XDocument.Load(docPath, LoadOptions.None);
            var reader = doc.CreateReader();
            var namespaceManager = new XmlNamespaceManager(reader.NameTable);
            namespaceManager.AddNamespace("x", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            namespaceManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

            // Check if the protocol was declared
            var decl = doc.Root?.XPathSelectElements($"//uap:Extension[@Category='windows.protocol']/uap:Protocol[@Name='{scheme}']", namespaceManager);

            var flag =  decl != null && decl.Any();
            return flag;
        }

        /// <summary>
        /// Performs an Authentication & protocol activation check and redirects activation to the correct application instance.
        /// </summary>
        /// <param name="skipShutDownOnActivation">If <c>true</c>, this application instance will not automatically be shut down. If set to
        /// <c>true</c> ensure you handle instance exit, or you'll end up with multiple instances running.</param>
        /// <returns><c>true</c> if the activation was redirected and this instance should be shut down, otherwise <c>false</c>.</returns>
        /// <remarks>
        /// The call to this method should be done preferably in the Program.Main method, or the application constructor. It must be called
        /// prior to using <see cref="AuthenticateAsync(Uri, Uri, CancellationToken)"/>
        /// </remarks>
        /// <seealso cref="AuthenticateAsync(Uri, Uri, CancellationToken)"/>
        public static bool CheckAuthenticationRedirectionActivation(bool skipShutDownOnActivation = false)
        {
            var currentApp = AppInstance.GetCurrent();
            var activatedEventArgs = currentApp?.GetActivatedEventArgs();
            return CheckAuthenticationRedirectionActivation(activatedEventArgs, skipShutDownOnActivation);
        }

        /// <summary>
        /// Performs an Authentication protocol activation check and redirects activation to the correct application instance.
        /// </summary>
        /// <param name="activatedEventArgs">The activation arguments</param>
        /// <param name="skipShutDownOnActivation">If <c>true</c>, this application instance will not automatically be shut down. If set to
        /// <c>true</c> ensure you handle instance exit, or you'll end up with multiple instances running.</param>
        /// <returns><c>true</c> if the activation was redirected and this instance should be shut down, otherwise <c>false</c>.</returns>
        /// <remarks>
        /// The call to this method should be done preferably in the Program.Main method, or the application constructor. It must be called
        /// prior to using <see cref="AuthenticateAsync(Uri, Uri, CancellationToken)"/>
        /// </remarks>
        /// <seealso cref="AuthenticateAsync(Uri, Uri, CancellationToken)"/>
        public static bool CheckAuthenticationRedirectionActivation(AppActivationArguments? activatedEventArgs, bool skipShutDownOnActivation = false)
        {
            _authencationCheckWasPerformed = true;

            if (activatedEventArgs is null)
            {
                return false;
            }

            if (activatedEventArgs.Kind != ExtendedActivationKind.Protocol)
            {
                return false;
            }

            var instance = AppInstance.GetInstances()
                                      .Where(x => string.Equals(x.Key, WebAuthenticatorConstants.CallbackUrl, StringComparison.OrdinalIgnoreCase))
                                      .FirstOrDefault();

            if (instance != null && instance.IsCurrent == false)
            {
                // Redirect to correct instance and close this one
                instance.RedirectActivationToAsync(activatedEventArgs).AsTask().Wait();

                var processId = Process.GetCurrentProcess().Id;

                if (!skipShutDownOnActivation)
                {
                    KillProcessAndChildren(processId);
                }

                return true;
            }

            return false;
        }

        private void CurrentAppInstance_Activated(object? sender, AppActivationArguments e)
        {
            if (e.Kind == ExtendedActivationKind.Protocol)
            {
                if (e.Data is IProtocolActivatedEventArgs protocolArgs)
                {
                    OnResumeCallback(protocolArgs.Uri);
                }
            }
        }

        private static bool CanHandleCallback(Uri expectedUrl, Uri callbackUrl)
        {
            if (!callbackUrl.Scheme.Equals(expectedUrl.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(expectedUrl.Host))
            {
                if (!callbackUrl.Host.Equals(expectedUrl.Host, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private bool OnResumeCallback(Uri callbackUri)
        {
            // If we aren't waiting on a task, don't handle the url
            if (_tcsResponse?.Task?.IsCompleted ?? true)
            {
                return false;
            }

            if (callbackUri == null)
            {
                _tcsResponse.TrySetCanceled();
                return false;
            }

            try
            {
                // Only handle schemes we expect
                if (CanHandleCallback(_currentRedirectUri, callbackUri) == false)
                {
                    _tcsResponse.TrySetException(new InvalidOperationException($"Invalid Redirect URI, detected `{callbackUri}` but expected a URI in the format of `{_currentRedirectUri}`"));
                    return false;
                }
                _tcsResponse?.TrySetResult(new WebAuthenticatorResult(callbackUri));
                return true;
            }
            catch (Exception ex)
            {
                _tcsResponse.TrySetException(ex);
                return false;
            }
        }

        private async Task<WebAuthenticatorResult> Authenticate(Uri authorizeUri, Uri callbackUri, CancellationToken cancellationToken)
        {
            if (_authencationCheckWasPerformed == false)
            {
                throw new InvalidOperationException("Authentication redirection check on app activation was not detected. Please make sure a call to WebAuthenticator.CheckAuthenticationRedirectionActivation was made during App creation.");
            }

            if (Helpers.IsAppPackaged == false)
            {
                throw new InvalidOperationException("The WebAuthenticator requires a packaged app with an AppxManifest");
            }

            if (IsUriProtocolDeclared(callbackUri.Scheme) == false)
            {
                throw new InvalidOperationException($"The URI Scheme {callbackUri.Scheme} is not declared in AppxManifest.xml");
            }

            // Cancel any previous task that's still pending
            if (_tcsResponse?.Task != null && !_tcsResponse.Task.IsCompleted)
            {
                _tcsResponse.TrySetCanceled();
            }

            _tcsResponse = new TaskCompletionSource<WebAuthenticatorResult>();
            _currentRedirectUri = callbackUri;

            var promptOptions = new LauncherOptions();
            var success = await Windows.System.Launcher.LaunchUriAsync(authorizeUri, promptOptions);

            return await _tcsResponse.Task;
        }

        /// <summary>
        /// Kill a process, and all of its children, grandchildren, etc.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();

            foreach (ManagementObject mo in moc)
            {
                var process = Convert.ToInt32(mo["ProcessID"]);
                KillProcessAndChildren(process);
            }

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (proc != null)
                {
                    proc.Kill();
                }
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }

    internal static class Helpers
    {
#pragma warning disable SA1203 // Constants should appear before fields
        private const long AppModelErrorNoPackage = 15700L;
#pragma warning restore SA1203 // Constants should appear before fields

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, System.Text.StringBuilder packageFullName);

        internal static bool IsAppPackaged
        {
            get {
                try
                {
                    // Application is MSIX packaged if it has an identity: https://learn.microsoft.com/en-us/windows/msix/detect-package-identity
                    int length = 0;
                    var sb = new System.Text.StringBuilder(0);
                    int result = GetCurrentPackageFullName(ref length, sb);
                    return result != AppModelErrorNoPackage;
                }
                catch
                {
                    return false;
                }
            }
        }

        internal static bool IsApplicationDataSupported => IsAppPackaged;
    }
}
