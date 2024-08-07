using Insurance.Api.Interfaces;
using Insurance.Api.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Insurance.Api.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessRulesService : IBusinessRulesService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory httpFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<IBusinessRulesService> logger;

        public BusinessRulesService(IHttpClientFactory httpFactory, ILogger<IBusinessRulesService> logger)
        {
            this.httpFactory = httpFactory;
            this.logger = logger;
        }

        public ProductDto GetProductDetails(int productID)
        {
            return GetResponse<ProductDto>($"/products/{productID}");
        }


        public ProductTypeDto GetProductTypeDetails(int productTypeID)
        {
            return GetResponse<ProductTypeDto>($"/product_types/{productTypeID}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private T GetResponse<T>(string requestUri) where T : class
        {
            try
            {
                var client = httpFactory.CreateClient("ProductClient");

                var json = client.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<T>(json);

                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return default(T);
            }
        }
    }
}
