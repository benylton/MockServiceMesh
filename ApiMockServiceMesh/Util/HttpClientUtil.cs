using ApiMockServiceMesh.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiMockServiceMesh.Util
{
    public class HttpClientUtil<T> : IDisposable where T : class
    {
        private HttpClient _client;

        public HttpClientUtil()
        {

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = new TimeSpan(0, 1, 1);
        }

        public async Task<T> Get(string api, string parameter)
        {
            var retorno = string.Empty;

            var urlConcat = $"{api}/{parameter}";

            var response = await _client.GetAsync(urlConcat);

            if (response.IsSuccessStatusCode)
            {
                retorno = await response.Content.ReadAsStringAsync();
            }

            return JsonConvert.DeserializeObject<T>(retorno);
        }

        public async Task<T> Post(string api, object obj)
        {
            var retorno = string.Empty;

            var json = JsonConvert.SerializeObject(obj);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(api, content);

            retorno = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(retorno);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
