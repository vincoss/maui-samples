using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    /// <summary>
    /// var status = await PermissionsHelper.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
    /// if (status != PermissionStatus.Granted)
    /// {
    ///    //show message
    /// }
    /// </summary>
    public static class PlatformPermissionsHelper
    {
        public static class PermissionsHelper
        {
            public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>()
                where TPermission : BasePermission, new()
            {
                return await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    TPermission permission = new TPermission();
                    var status = await permission.CheckStatusAsync();
                    if (status != PermissionStatus.Granted)
                    {
                        status = await permission.RequestAsync();
                    }

                    return status;
                });
            }
        }
    }
}
