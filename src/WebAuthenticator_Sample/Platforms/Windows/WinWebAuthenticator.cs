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
using Windows.ApplicationModel.Activation;


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

        private static IDictionary<string, string> GetState(AppActivationArguments activatedEventArgs)
        {
            if (activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol &&
                activatedEventArgs.Data is IProtocolActivatedEventArgs protocolArgs)
            {
                return GetState(protocolArgs);
            }
            return null;
        }

        private static IDictionary<string, string> GetState(IProtocolActivatedEventArgs protocolArgs)
        {
            var preferenceString = Preferences.Get(WebAuthenticatorResult.WinWebAuthenticatorStateKey, null);
            if (string.IsNullOrWhiteSpace(preferenceString) == false)
            {
                var dict = GetQueryParameters(preferenceString);
                return dict;
            }

            var dictQuery = GetQueryParameters(protocolArgs.Uri.Query);

            if (dictQuery.Keys.Count > 0)
            {
                Preferences.Set(WebAuthenticatorResult.WinWebAuthenticatorStateKey, protocolArgs.Uri.Query);
            }

            return dictQuery;
        }
        
        private static bool _oauthCheckWasPerformed;

        /// <summary>
        /// Performs an OAuth protocol activation check and redirects activation to the correct application instance.
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
            var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent()?.GetActivatedEventArgs();
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
            _oauthCheckWasPerformed = true;

            if (activatedEventArgs is null)
            {
                return false;
            }
            if (activatedEventArgs.Kind != Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
            {
                return false;
            }

            //var id = dic[WebAuthenticatorResult.AppInstanceIdKey];
            var id = @"com.companyname.webauthenticator.sample://callback";
            var instance = Microsoft.Windows.AppLifecycle.AppInstance.GetInstances().Where(i => i.Key == id).FirstOrDefault();

            if (instance is not null && !instance.IsCurrent)
            {
                // Redirect to correct instance and close this one
                instance.RedirectActivationToAsync(activatedEventArgs).AsTask().Wait();

                var pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;

                if (!skipShutDownOnActivation)
                {
                    //if (pid2 > 0)
                    //{
                    //    KillProcessAndChildren(pid2);
                    //}

                    KillProcessAndChildren(pid1);


                    //System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                return true;
            }

            return false;

            //var dic = GetState(activatedEventArgs);

            //if (dic.ContainsKey(WebAuthenticatorResult.AppInstanceIdKey))
            //{
            //    //var id = dic[WebAuthenticatorResult.AppInstanceIdKey];
            //    var id = @"com.companyname.webauthenticator.sample://callback";
            //    var instance = Microsoft.Windows.AppLifecycle.AppInstance.GetInstances().Where(i => i.Key == id).FirstOrDefault();

            //    if (instance is not null && !instance.IsCurrent)
            //    {
            //        // Redirect to correct instance and close this one
            //        instance.RedirectActivationToAsync(activatedEventArgs).AsTask().Wait();

            //        var pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;

            //        if (!skipShutDownOnActivation)
            //        {
            //            //if (pid2 > 0)
            //            //{
            //            //    KillProcessAndChildren(pid2);
            //            //}

            //            KillProcessAndChildren(pid1);


            //            //System.Diagnostics.Process.GetCurrentProcess().Kill();

            //        }
            //        return true;
            //    }
            //}
            //else
            //{
            //    var thisInstance = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent();
            //    if (string.IsNullOrEmpty(thisInstance.Key))
            //    {
            //        Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey(Guid.NewGuid().ToString());
            //    }
            //}
            //return false;
        }

        private void CurrentAppInstance_Activated(object? sender, Microsoft.Windows.AppLifecycle.AppActivationArguments e)
        {
            if (e.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
            {
                if (e.Data is IProtocolActivatedEventArgs protocolArgs)
                {
                    ResumeSignin(protocolArgs.Uri, _taskId);
                    //var vals = GetState(protocolArgs);
                    //if (vals is not null && vals[WebAuthenticatorResult.SigninIdKey] is string signinId)
                    //{
                    //    ResumeSignin(protocolArgs.Uri, signinId);
                    //}
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

        public static Uri AppendQueryParameters(Uri uri, IDictionary<string, string> parameters)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (parameters == null || parameters.Count <= 0)
            {
                return uri;
            }

            const string returnUrl = "returnUrl";
            var uriBuilder = new UriBuilder(uri);
            var query =  HttpUtility.ParseQueryString(uriBuilder.Query);
            var returnUrlQuery = HttpUtility.ParseQueryString(query[returnUrl]);
            var empty = HttpUtility.ParseQueryString("");

            if (returnUrlQuery.HasKeys() == false)
            {
                return uri;
            }

            foreach (var pair in parameters)
            {
                empty[pair.Key] = pair.Value;
            }

            foreach(var key in returnUrlQuery.AllKeys)
            {
                empty[key] = returnUrlQuery[key];
            }

            query[returnUrl] = empty.ToString();
            uriBuilder.Query = query.ToString();
            var updateUri = uriBuilder.Uri;

            var decode = DecodeUrlString(updateUri.ToString());
            var newUri = new Uri(decode);

            return newUri;
        }

        public static IDictionary<string, string> GetQueryParameters(string url)
        {
            var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            //const string returnUrl = "returnUrl";
            //var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(url);
            //var returnUrlQuery = HttpUtility.ParseQueryString(query[returnUrl]);

            foreach (var key in query.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                parameters.Add(key, query[key]);
            }

            return parameters;
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        private async Task<WebAuthenticatorResult> Authenticate(Uri authorizeUri, Uri callbackUri, CancellationToken cancellationToken)
        {
            if (_oauthCheckWasPerformed == false)
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


            Preferences.Remove(WebAuthenticatorResult.WinWebAuthenticatorStateKey);


            //var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            //parameters.Add(WebAuthenticatorResult.AppInstanceIdKey, Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Key);
            //parameters.Add(WebAuthenticatorResult.SigninIdKey, taskId);
            //authorizeUri = AppendQueryParameters(authorizeUri, parameters);

            var tcs = new TaskCompletionSource<Uri>();
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    if (_tasks.ContainsKey(_taskId))
                        _tasks.Remove(_taskId);
                });

                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            var promptOptions = new Windows.System.LauncherOptions();
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
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
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

    /// <summary>
    /// Web Authenticator result parsed from the callback Url.
    /// </summary>
    /// <seealso cref="WebAuthenticator"/>
    public class WebAuthenticatorResult
    {
        public const string StateKey = "state";
        public const string AppInstanceIdKey = "appInstanceId";
        public const string SigninIdKey = "signinId";
        public const string WinWebAuthenticatorStateKey = "WinWebAuthenticatorStateKey";
        public const string WinWebAuthenticatorProcessIdKey = "WinWebAuthenticatorProcessIdKey";

        public static string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result)
        {
            try
            {
                var queryDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                var parametersDict = result.Properties
                                           .Where(x => x.Key.Equals(StateKey, StringComparison.OrdinalIgnoreCase) == false)
                                           .Select(x => x);

                foreach (var pair in parametersDict)
                {
                    queryDict.Add(pair.Key, pair.Value);
                }

                var builder = new UriBuilder(redirectUrl.Trim('/'));
                var dicta = ParseValues(result.Properties, StateKey);

                if (dicta.ContainsKey(StateKey))
                {
                    var state = dicta[StateKey];

                    queryDict.Add(StateKey, state);

                    var queryString = string.Join('&', queryDict.Select(q => $"{q.Key}={q.Value}"));
                    builder.Query = queryString;
                }

                var url = builder.ToString();
                return url;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public static string FixJsonValues(string json)
        {
            if (json.IndexOf('"') >= 0)
            {
                return json;
            }

            var dic = new Dictionary<string, string?>();
            var pairs = json.Trim().Trim('}', '{').Split(',');

            foreach (var pair in pairs)
            {
                var values = pair.Split(':');
                string? p = null;
                string? v = null;

                if (values.Length > 0)
                {
                    p = values[0];
                }

                if (values.Length > 1)
                {
                    v = values[1];
                }

                if (string.IsNullOrWhiteSpace(p) == false)
                {
                    dic.Add(p, v);
                }
            }

            var str = System.Text.Json.JsonSerializer.Serialize(dic);
            return str;
        }

        public static IDictionary<string, string> ParseValues(IDictionary<string, string> dictVals, string key)
        {
            if (dictVals == null)
            {
                dictVals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            if (dictVals.ContainsKey(key) == false)
            {
                return dictVals;
            }

            var str = dictVals[key].ToString();
            var json = FixJsonValues(str);
            var dict = JsonSerializer.Deserialize<IDictionary<string, string>>(json);

            return dict;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAuthenticatorResult"/> class.
        /// </summary>
        /// <param name="callbackUrl">Callback url</param>
        public WebAuthenticatorResult(Uri callbackUrl)
        {
            var dictVals = WinWebAuthenticator.GetQueryParameters(callbackUrl.ToString());
            Properties = dictVals;
        }

        /// <summary>
        /// The dictionary of key/value pairs parsed form the callback URI's querystring.
        /// </summary>
        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the value for the <c>access_token</c> key.
        /// </summary>
        /// <value>Access Token parsed from the callback URI <c>access_token</c> parameter.</value>
        public string AccessToken => GetValue("access_token");

        /// <summary>
        /// Gets the value for the <c>refresh_token</c> key.
        /// </summary>
        /// <value>Refresh Token parsed from the callback URI <c>refresh_token</c> parameter.</value>
        public string RefreshToken => GetValue("refresh_token");

        /// <summary>
        /// Gets the value for the <c>id_token</c> key.
        /// </summary>
        public string IdToken => GetValue("id_token");

        /// <summary>
        /// Gets the expiry date as calculated by the timestamp of when the result was created plus the value in seconds for the <c>expires_in</c> key.
        /// </summary>
        /// <value>Timestamp of the creation of the object instance plus the <c>expires_in</c> seconds parsed from the callback URI.</value>
        public DateTimeOffset? RefreshTokenExpiresIn
        {
            get {
                if (Properties.TryGetValue("refresh_token_expires_in", out var value))
                {
                    if (int.TryParse(value, out var i))
                        return DateTimeOffset.UtcNow.AddSeconds(i);
                }

                return null;
            }
        }

        /// <summary>
        /// The expiry date as calculated by the timestamp of when the result was created plus the value in seconds for the <c>expires_in</c> key.
        /// </summary>
        /// <value>Timestamp of the creation of the object instance plus the <c>expires_in</c> seconds parsed from the callback URI.</value>
        public DateTimeOffset? ExpiresIn
        {
            get {
                if (Properties.TryGetValue("expires_in", out var value))
                {
                    if (int.TryParse(value, out var i))
                        return DateTimeOffset.UtcNow.AddSeconds(i);
                }

                return null;
            }
        }

        private string GetValue(string key)
        {
            if (Properties.TryGetValue(key, out var value))
                return value;
            return null;
        }
    }
}
