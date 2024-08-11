using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductDataService : IProductDataService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<IProductDataService> logger;

        public ProductDataService(IHttpClientFactory httpFactory, ILogger<IProductDataService> logger)
        {
            this.httpFactory = httpFactory;
            this.logger = logger;
        }

        public async Task<ProductDto> GetProductDetails(int productID)
        {
            return await GetResponse<ProductDto>($"/products/{productID}");
        }


        public async Task<ProductTypeDto> GetProductTypeDetails(int productTypeID)
        {
            return await GetResponse<ProductTypeDto>($"/product_types/{productTypeID}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private async Task<T> GetResponse<T>(string requestUri) where T : class
        {
            try
            {
                var client = httpFactory.CreateClient("ProductClient");

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
