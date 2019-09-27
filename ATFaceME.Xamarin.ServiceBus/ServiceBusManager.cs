using ApiBackend.Transversal.DTOs.PLC;
using Newtonsoft.Json;
using PCLCrypto;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.ServiceBus
{
    public class ServiceBusManager
    {

        public const string ServiceBusNamespace = "XXXXXXX"; //encripted for recurity reasons

        public const string BaseServiceBusAddress = "https://" + ServiceBusNamespace + ".servicebus.windows.net/";

        /// <summary>
        /// The get shared access signature token.
        /// </summary>
        /// <param name="sasKeyName">
        /// The shared access signature key name.
        /// </param>
        /// <param name="sasKeyValue">
        /// The shared access signature key value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSasToken(string sasKeyName= "RootManageSharedAccessKey", string sasKeyValue= "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")
        {
            var expiry = GetExpiry();
            var stringToSign = WebUtility.UrlEncode(BaseServiceBusAddress) + "\n" + expiry;
            try
            {
                var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha256);
                var hasher = algorithm.CreateHash(Encoding.UTF8.GetBytes(sasKeyValue));
                hasher.Append(Encoding.UTF8.GetBytes(stringToSign));
                var mac = hasher.GetValueAndReset();
                var signature = Convert.ToBase64String(mac);

                var sasToken = string.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", WebUtility.UrlEncode(BaseServiceBusAddress), WebUtility.UrlEncode(signature), expiry, sasKeyName);
                return sasToken;
            }
            catch(Exception ex)
            { return null; }
           
        }

        /// <summary>
        /// Posts an order data transfer object to queue.
        /// </summary>
        /// <param name="orderDto">
        /// The order data transfer object.
        /// </param>
        /// <param name="serviceBusNamespace">
        /// The service bus namespace.
        /// </param>
        /// <param name="sasKeyName">
        /// The shared access signature key name.
        /// </param>
        /// <param name="sasKey">
        /// The shared access signature key.
        /// </param>
        /// <param name="queue">
        /// The queue.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<HttpResponseMessage> PostTokenToQueue(string idToken, 
            string serviceBusNamespace,
            string queue,
            string sasKeyName = "RootManageSharedAccessKey", 
            string sasKeyValue = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" //encripted for security reasons
            )
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseServiceBusAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var token = GetSasToken(sasKeyName, sasKeyValue);
                client.DefaultRequestHeaders.Add("Authorization", token);
                BrokeredMessage message = new BrokeredMessage();
                message.idToken = idToken;
                HttpContent content = new StringContent(JsonConvert.SerializeObject(message));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var path = BaseServiceBusAddress + queue + "/messages";

                return await client.PostAsync(path, content);
            }
        }

        /// <summary>
        ///     Gets the expiry for a shared access signature token
        /// </summary>
        /// <returns>
        ///     The <see cref="string" /> expiry.
        /// </returns>
        private static string GetExpiry()
        {
            var sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToString((int)sinceEpoch.TotalSeconds + 3600);
        }
    }
}
