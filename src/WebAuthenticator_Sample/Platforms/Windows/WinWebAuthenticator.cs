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


public static class LoggerExtensions
{
    public static IDictionary<string, string?> ToDictionary(this NameValueCollection items)
    {
        var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        if (items != null)
        {
            foreach (var key in items.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;
                dict.Add(key.Trim(), items[key]?.Trim());
            }
        }

        return dict;
    }
}

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
        public static Task<WebAuthenticatorResult> AuthenticateAsync(Uri authorizeUri, Uri callbackUri) => Instance.Authenticate(authorizeUri, callbackUri, CancellationToken.None);

        private Dictionary<string, TaskCompletionSource<Uri>> tasks = new Dictionary<string, TaskCompletionSource<Uri>>();

        private static readonly WinWebAuthenticator Instance = new WinWebAuthenticator();

        static WinWebAuthenticator()
        {
        }

        private WinWebAuthenticator()
        {
            Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Activated += CurrentAppInstance_Activated;
        }

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

        private static System.Collections.Specialized.NameValueCollection? GetState(Microsoft.Windows.AppLifecycle.AppActivationArguments activatedEventArgs)
        {
            if (activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol &&
                activatedEventArgs.Data is IProtocolActivatedEventArgs protocolArgs)
            {
                return GetState(protocolArgs);
            }
            return null;
        }


        private static NameValueCollection? GetState(IProtocolActivatedEventArgs protocolArgs)
        {
            var strValue = Preferences.Get(WebAuthenticatorResult.WinWebAuthenticatorStateKey, null);
            if (strValue != null)
            {
                var json = WebAuthenticatorResult.FixJsonValues(strValue);
                var dict = JsonSerializer.Deserialize<IDictionary<string, string>>(json);

                var vals1 = new NameValueCollection();

                if (dict != null)
                {
                    foreach (var kvp in dict)
                    {
                        vals1.Add(kvp.Key, kvp.Value);
                    }
                }

                return vals1;
            }

            NameValueCollection? vals = null;
            try
            {
                vals = System.Web.HttpUtility.ParseQueryString(protocolArgs.Uri.Query);
            }
            catch { }
            try
            {
                if (vals is null || !(vals["state"] is string))
                {
                    var fragment = protocolArgs.Uri.Fragment;
                    if (fragment.StartsWith("#"))
                    {
                        fragment = fragment.Substring(1);
                    }
                    vals = System.Web.HttpUtility.ParseQueryString(fragment);
                }
            }
            catch { }
            if (vals != null && vals["state"] is string state)
            {
                try
                {
                    var dictVals = vals.ToDictionary();
                    var str = dictVals["state"].ToString();
                    var json = WebAuthenticatorResult.FixJsonValues(str);
                    var dict = JsonSerializer.Deserialize<IDictionary<string, string>>(json);

                    // Keep state
                    Preferences.Set(WebAuthenticatorResult.WinWebAuthenticatorStateKey, json);

                    var vals2 = new NameValueCollection();

                    if (dict != null)
                    {
                        foreach (var kvp in dict)
                        {
                            vals2.Add(kvp.Key, kvp.Value);
                        }
                    }

                    //if (string.IsNullOrWhiteSpace(state) == false)
                    //{
                    //    var pairs = state.Replace("{", "").Replace("}", "").Split(',');

                    //    if (pairs.Any())
                    //    {
                    //        foreach (var pair in pairs)
                    //        {
                    //            var p2 = pair.Split(new[] { ':' });
                    //            if (p2.Any() && p2.Count() > 1)
                    //            {
                    //                var key = p2[0];
                    //                var value = p2[1]?.ToString();
                    //                vals2.Add(key, value);
                    //            }
                    //        }
                    //    }
                    //}

                    return vals2;

                    //var jsonObject = System.Text.Json.Nodes.JsonObject.Parse(state) as JsonObject;
                    //if (jsonObject is not null)
                    //{
                    //    NameValueCollection vals2 = new NameValueCollection(jsonObject.Count);
                    //    if (jsonObject.ContainsKey("appInstanceId") && jsonObject["appInstanceId"] is JsonValue jvalue && jvalue.TryGetValue<string>(out string? value))
                    //        vals2.Add("appInstanceId", value);
                    //    if (jsonObject.ContainsKey("signinId") && jsonObject["signinId"] is JsonValue jvalue2 && jvalue2.TryGetValue<string>(out string? value2))
                    //        vals2.Add("signinId", value2);
                    //    return vals2;
                    //}
                }
                catch { }
            }
            return null;
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
                return false;
            if (activatedEventArgs.Kind != Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
                return false;
            var state = GetState(activatedEventArgs);
            var dic = LoggerExtensions.ToDictionary(state);


            if (dic.ContainsKey(WebAuthenticatorResult.AppInstanceIdKey))
            {
                var id = dic[WebAuthenticatorResult.AppInstanceIdKey];
                var instance = Microsoft.Windows.AppLifecycle.AppInstance.GetInstances().Where(i => i.Key == id).FirstOrDefault();

                if (instance is not null && !instance.IsCurrent)
                {
                    // Redirect to correct instance and close this one
                    instance.RedirectActivationToAsync(activatedEventArgs).AsTask().Wait();

                    var pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;
                    //  int pid2 = Preferences.Get(WebAuthenticatorResult.WinWebAuthenticatorProcessIdKey, 0);

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
            }
            else
            {
                var thisInstance = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent();
                if (string.IsNullOrEmpty(thisInstance.Key))
                {
                    Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey(Guid.NewGuid().ToString());
                }
            }
            return false;
        }

        private void CurrentAppInstance_Activated(object? sender, Microsoft.Windows.AppLifecycle.AppActivationArguments e)
        {
            if (e.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
            {
                if (e.Data is IProtocolActivatedEventArgs protocolArgs)
                {
                    var vals = GetState(protocolArgs);
                    if (vals is not null && vals[WebAuthenticatorResult.SigninIdKey] is string signinId)
                    {
                        ResumeSignin(protocolArgs.Uri, signinId);
                    }
                }
            }
        }

        private void ResumeSignin(Uri callbackUri, string signinId)
        {
            if (signinId != null && tasks.ContainsKey(signinId))
            {
                var task = tasks[signinId];
                tasks.Remove(signinId);
                task.TrySetResult(callbackUri);
            }
            else
            {
                var task = tasks.FirstOrDefault();
                tasks.Remove(signinId);
                task.Value.TrySetResult(callbackUri);
            }
        }

        private async Task<WebAuthenticatorResult> Authenticate(Uri authorizeUri, Uri callbackUri, CancellationToken cancellationToken)
        {
            if (!_oauthCheckWasPerformed)
            {
                throw new InvalidOperationException("OAuth redirection check on app activation was not detected. Please make sure a call to WebAuthenticator.CheckOAuthRedirectionActivation was made during App creation.");
            }

            if (!Helpers.IsAppPackaged)
            {
                throw new InvalidOperationException("The WebAuthenticator requires a packaged app with an AppxManifest");
            }

            if (!IsUriProtocolDeclared(callbackUri.Scheme))
            {
                throw new InvalidOperationException($"The URI Scheme {callbackUri.Scheme} is not declared in AppxManifest.xml");
            }

            var taskId = Guid.NewGuid().ToString();

            var stateJson = new JsonObject
            {
                { WebAuthenticatorResult.AppInstanceIdKey, Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Key },
                { WebAuthenticatorResult.SigninIdKey, taskId },
                { WebAuthenticatorResult.StateKey, Guid.NewGuid().ToString() }
            };

            var uriBuilder = new UriBuilder(callbackUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add(WebAuthenticatorResult.StateKey, stateJson.ToJsonString());
            query.Add("redirect_uri", callbackUri.ToString());

            uriBuilder.Query = query.ToString();
            var newUrl = uriBuilder.ToString();
            authorizeUri = new Uri($"{authorizeUri}?returnUrl={newUrl}");

            //var g = Guid.NewGuid();
            //var taskId = g.ToString();
            //UriBuilder b = new UriBuilder(authorizeUri);

            //var query = System.Web.HttpUtility.ParseQueryString(authorizeUri.Query);
            //var stateJson = new JsonObject
            //{
            //    { WebAuthenticatorResult.AppInstanceIdKey, Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Key },
            //    { WebAuthenticatorResult.SigninIdKey, taskId }
            //};
            //if (query[WebAuthenticatorResult.StateKey] is string oldstate && !string.IsNullOrEmpty(oldstate))
            //{
            //    stateJson[WebAuthenticatorResult.StateKey] = oldstate;
            //}

            //var strJson = stateJson.ToJsonString();
            //query[WebAuthenticatorResult.StateKey] = strJson;
            //b.Query = query.ToString();
            //authorizeUri = b.Uri;

            var tcs = new TaskCompletionSource<Uri>();
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    if (tasks.ContainsKey(taskId))
                        tasks.Remove(taskId);
                });
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            //var process = new System.Diagnostics.Process();
            //process.StartInfo.FileName = "rundll32.exe";
            //process.StartInfo.Arguments = $"url.dll,FileProtocolHandler {authorizeUri}";
            //process.StartInfo.UseShellExecute = true;
            //process.Start();

            //Preferences.Set(WebAuthenticatorResult.WinWebAuthenticatorProcessIdKey, process.Id);

            var promptOptions = new Windows.System.LauncherOptions();
            var success = await Windows.System.Launcher.LaunchUriAsync(authorizeUri, promptOptions);

            tasks.Add(taskId, tcs);
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
            var str = string.Empty;

            if (!string.IsNullOrEmpty(callbackUrl.Fragment))
            {
                str = callbackUrl.Fragment.Substring(1);
            }
            else if (!string.IsNullOrEmpty(callbackUrl.Query))
            {
                str = callbackUrl.Query;
            }

            var query = System.Web.HttpUtility.ParseQueryString(str);
            var dictVals = query.ToDictionary();

            foreach (var pair in dictVals)
            {
                if (string.Equals(StateKey, pair.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var dict = ParseValues(dictVals, StateKey);
                    var propStr = JsonSerializer.Serialize(dict); ;
                    Properties[pair.Key] = propStr;
                }
                else
                {
                    Properties[pair.Key] = query[pair.Key] ?? null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAuthenticatorResult"/> class.
        /// </summary>
        /// <param name="values">Values from the authentication callback url</param>
        public WebAuthenticatorResult(Dictionary<string, string> values)
        {
            foreach (var pair in values)
            {
                var value = pair.Value?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = null;
                }

                Properties[pair.Key?.Trim()] = value;
            }
        }

        /// <summary>
        /// The dictionary of key/value pairs parsed form the callback URI's querystring.
        /// </summary>
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

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
