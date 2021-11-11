using StartupConfigurationSample.Interfaces;
using System;
using System.Net.Http;


namespace StartupConfigurationSample.Services
{
    public class HttpService : IHttpService
    {
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
    }
}