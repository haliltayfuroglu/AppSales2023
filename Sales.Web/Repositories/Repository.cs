using System.Text;
using System.Text.Json;

namespace Sales.Web.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;


        //Here i am no used json ignore in the properties
        private JsonSerializerOptions _serializerDefaultOptions => new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(responseHttp, _serializerDefaultOptions);

                return new HttpResponseWrapper<T>(response, false, responseHttp);

            }

            return new HttpResponseWrapper<T>(default, true, responseHttp);

        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);

            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContent);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _serializerDefaultOptions);

                return new HttpResponseWrapper<TResponse>(default, false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }


        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }
    }
}
