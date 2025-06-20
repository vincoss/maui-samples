using Microsoft.Maui.ApplicationModel;
using Microsoft.Windows.AppLifecycle;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WebAuthenticator_Sample;
using Windows.ApplicationModel.Activation;
using Windows.System;


namespace OAuth_Samples
{
    /// https://learn.microsoft.com/en-us/windows/uwp/launch-resume/handle-uri-activation
    /// <summary>
    /// Handles OAuth redirection to the system browser and re-activation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Your app must be configured for OAuth. In you app package's <c>Package.appxmanifest</c> under Declarations, add a 
    /// Protocol declaration and add the scheme you registered for your application's oauth redirect url under "Name".
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
        /// <remarks>Prior to calling this, a call to <see cref="CheckOAuthRedirectionActivation(bool)"/> must be made during application startup.</remarks>
        /// <seealso cref="CheckOAuthRedirectionActivation(bool)"/>
        public static Task<WebAuthenticatorResult> AuthenticateAsync(Uri authorizeUri, Uri callbackUri) => _instance.Authenticate(authorizeUri, callbackUri, CancellationToken.None);

        private Dictionary<string, TaskCompletionSource<Uri>> _tasks = new Dictionary<string, TaskCompletionSource<Uri>>();

        private static string _taskId = Guid.NewGuid().ToString();
        private static bool _authencationCheckWasPerformed;


        TaskCompletionSource<WebAuthenticatorResult> tcsResponse = null;


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
        public static bool CheckOAuthRedirectionActivation(bool skipShutDownOnActivation = false)
        {
            var currentApp = AppInstance.GetCurrent();
            var activatedEventArgs = currentApp?.GetActivatedEventArgs();
            return CheckOAuthRedirectionActivation(activatedEventArgs, skipShutDownOnActivation);
        }

        /// <summary>
        /// Performs an OAuth protocol activation check and redirects activation to the correct application instance.
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
        public static bool CheckOAuthRedirectionActivation(AppActivationArguments? activatedEventArgs, bool skipShutDownOnActivation = false)
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
                    ResumeSignin(protocolArgs.Uri, _taskId);
                }
            }
        }

        private void ResumeSignin(Uri callbackUri, string signinId)
        {
            if (signinId != null && _tasks.ContainsKey(signinId))
            {
                var task = _tasks[signinId];
                _tasks.Remove(signinId);
                task.TrySetResult(callbackUri);
            }
            else
            {
                var task = _tasks.FirstOrDefault();
                _tasks.Remove(signinId);
                task.Value.TrySetResult(callbackUri);
            }
        }

        private async Task<WebAuthenticatorResult> Authenticate(Uri authorizeUri, Uri callbackUri, CancellationToken cancellationToken)
        {
            if (_authencationCheckWasPerformed == false)
            {
                throw new InvalidOperationException("OAuth redirection check on app activation was not detected. Please make sure a call to WebAuthenticator.CheckOAuthRedirectionActivation was made during App creation.");
            }

            if (Helpers.IsAppPackaged == false)
            {
                throw new InvalidOperationException("The WebAuthenticator requires a packaged app with an AppxManifest");
            }

            if (IsUriProtocolDeclared(callbackUri.Scheme) == false)
            {
                throw new InvalidOperationException($"The URI Scheme {callbackUri.Scheme} is not declared in AppxManifest.xml");
            }

            // Preferences.Remove(WebAuthenticatorResult.WinWebAuthenticatorStateKey);

            tcsResponse = new TaskCompletionSource<WebAuthenticatorResult>();

            var tcs = new TaskCompletionSource<Uri>();
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    if (_tasks.ContainsKey(_taskId))
                    {
                        _tasks.Remove(_taskId);
                    }
                });

                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            var promptOptions = new LauncherOptions();
            var success = await Windows.System.Launcher.LaunchUriAsync(authorizeUri, promptOptions);

            _tasks.Add(_taskId, tcs);
            var uri = await tcs.Task.ConfigureAwait(false);
            return new WebAuthenticatorResult(uri);
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
