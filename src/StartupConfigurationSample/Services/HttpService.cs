using StartupConfigurationSample.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StartupConfigurationSample.Services
{
    public class HttpService : IHttpService
    {
        // Other sample
        //private readonly IHttpClientFactory _clientFactory;

        //public HttpService(IHttpClientFactory httpClientFactory)
        //{
        //    _clientFactory = httpClientFactory;
        //    var client = _clientFactory.CreateClient();

        //    if (client == null)
        //    {
        //        throw new Exception(nameof(client));
        //    }
        //}

        private readonly HttpClient _httpClient;
        public HttpService(HttpClient httpClient)
        {
                _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<int> GetAsync()
        {
            var result = await _httpClient.GetAsync("https://github.com/vincoss");
            return (int)result.StatusCode;
        }
    }
}