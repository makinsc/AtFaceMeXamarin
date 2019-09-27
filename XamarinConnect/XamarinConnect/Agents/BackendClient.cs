using ApiBackend.Transversal.DTOs.PLC;
using ApiBackend.Transversal.DTOs.PLC.RequestDTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.Core.Agents
{
    public class BackendClient : IAgentClient
    {
        public Task<TOutput> CallHttp<TInput, TOutput>(string endpoint, HttpVerbs method, string token, TInput content, string skipToken)
        {
            return CallWebApiAsync<TInput, TOutput>(endpoint, method, token, content, skipToken);
        }
      

        private async Task<TOutput> CallWebApiAsync<TInput, TOutput>(string endpoint, HttpVerbs method, string token, TInput input, string skipToken)
        {
            HttpClient httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (!string.IsNullOrEmpty(skipToken))
            {
                httpClient.DefaultRequestHeaders.Add(Endpoints.HeaderSkipToken,skipToken);
            }
            HttpContent content = null;

            string url = string.Format("{0}{1}", Endpoints.urlBase, endpoint);
            string serialized = string.Empty;

            if (input != null)
            {
                if (input is Stream)
                {
                    content = new StreamContent(input as Stream);
                }
                else
                {
                    serialized = JsonConvert.SerializeObject(input);
                    content = new StringContent(serialized, Encoding.UTF8, "application/json");
                }
            }

            HttpResponseMessage response = null;
            switch (method)
            {
                case HttpVerbs.GET:
                    response = await httpClient.GetAsync(url);
                    break;

                case HttpVerbs.POST:
                    response = await httpClient.PostAsync(url, content);
                    break;

                default:
                    break;
            }

            return await ManageBackendResponse<TOutput>(url, response);
        }

        private async Task<TOutput> ManageBackendResponse<TOutput>(string url, HttpResponseMessage response)
        {
            var task = response.Content?.ReadAsStringAsync();
            var responseText = await (task ?? Task.FromResult<string>(null));

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<TOutput>(responseText, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                return result;
            }
            else if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return default(TOutput);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("No Autorizado. " + responseText);
            }
            else
            {
                throw new Exception(JsonConvert.SerializeObject(new { url = url, statusCode = response.StatusCode, response = responseText }));
            }
        }
    }
}
