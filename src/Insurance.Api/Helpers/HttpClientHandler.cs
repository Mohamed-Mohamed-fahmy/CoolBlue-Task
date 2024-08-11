using Insurance.Api.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Insurance.Api.Helpers
{
    public class HttpClientHandler : IHttpClientHandler
    {
        private readonly IHttpClientFactory httpFactory;
        private readonly ILogger<IHttpClientHandler> logger;

        public HttpClientHandler(IHttpClientFactory httpFactory, ILogger<IHttpClientHandler> logger)
        {
            this.httpFactory = httpFactory;
            this.logger = logger;
        }

        public async Task<T> GetResponse<T>(string requestUri) where T : class
        {
            try
            {
                var client = this.httpFactory.CreateClient("ProductClient");

                var json = await client.GetAsync(requestUri);
                var content = await json.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<T>(content);

                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
