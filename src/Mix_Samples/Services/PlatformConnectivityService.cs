using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix_Samples.Services
{
    // https://docs.microsoft.com/en-us/xamarin/essentials/connectivity?tabs=android
    public interface IConnectivityService
    {
        bool IsConnected { get; }
    }

    public class PlatformConnectivityService : IConnectivityService
    {
        private bool HasInternetConnection()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            return false;
        }

        public bool IsConnected
        {
            get { return HasInternetConnection(); }
        }
    }
}
