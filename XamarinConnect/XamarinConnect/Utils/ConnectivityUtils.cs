using Plugin.Connectivity;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.Core.Utils
{
    public class ConnectivityUtils : IDisposable
    {
        private HttpWebRequest request;

        // It´s used to change the requested default url to avoid the cache
        private int valueDummy = 0;

        ~ConnectivityUtils()
        {
            Dispose();
        }


        /// <summary>
        /// Indicates if the device has internet connectivity
        /// </summary>
        public bool CheckConnectivity()
        {
            return CrossConnectivity.Current.IsConnected;
        }


        public void Dispose()
        {
            request = null;
            GC.SuppressFinalize(this);
        }
    }
}
