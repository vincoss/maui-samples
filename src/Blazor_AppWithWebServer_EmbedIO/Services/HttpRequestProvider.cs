using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public interface IHttpRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationToken cancellationToken = default);

        Task<TResult> PostAsync<TResult>(string uri, string data, string token = "", CancellationToken cancellationToken = default);
    }

    public class HttpRequestProvider : IHttpRequestProvider
    {
        private JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nameof(uri))) throw new ArgumentNullException(nameof(uri));

            HttpClient httpClient = CreateHttpClient(token);
            HttpResponseMessage response = await httpClient.GetAsync(uri, cancellationToken);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync(cancellationToken);

            if(typeof(TResult) == typeof(string))
            {
                return (TResult)Convert.ChangeType(serialized, typeof(TResult));
            }

            TResult result = await Task.Run(() => JsonSerializer.Deserialize<TResult>(serialized, _serializeOptions));

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string token = "", CancellationToken cancellationToken = default)
        {
            HttpClient httpClient = CreateHttpClient(token);

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content
            };

            using (var response = await httpClient.SendAsync(message, cancellationToken))
            {
                await HandleResponse(response);
                string serialized = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(serialized))
                {
                    return default(TResult);
                }

                TResult result = await Task.Run(() => JsonSerializer.Deserialize<TResult>(serialized, _serializeOptions));

                return result;
            }
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(15);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (string.IsNullOrWhiteSpace(token) == false)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"StatusCode: {response.StatusCode}{Environment.NewLine}{content}");
            }
        }
    }
}
