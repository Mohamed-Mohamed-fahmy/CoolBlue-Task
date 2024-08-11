using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using Microsoft.Extensions.Logging;
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
        private readonly IHttpClientHandler httpClientHandler;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<IProductDataService> logger;

        public ProductDataService(IHttpClientHandler httpClientHandler, ILogger<IProductDataService> logger)
        {
            this.httpClientHandler = httpClientHandler;
            this.logger = logger;
        }

        public async Task<ProductDto> GetProductDetails(int productID)
        {
            this.logger.LogInformation($"Retrieving product details for productId : {productID}");
            return await this.httpClientHandler.GetResponse<ProductDto>($"/products/{productID}");
        }


        public async Task<ProductTypeDto> GetProductTypeDetails(int productTypeID)
        {
            this.logger.LogInformation($"Retrieving product type details for productTypeId : {productTypeID}");
            return await this.httpClientHandler.GetResponse<ProductTypeDto>($"/product_types/{productTypeID}");
        }

    }
}
