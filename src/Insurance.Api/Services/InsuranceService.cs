using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductDataService productDataService;
        private readonly ISurchargeService surchargeService;
        private readonly ILogger<InsuranceService> logger;

        public InsuranceService(IProductDataService productDataService, ISurchargeService surchargeService, ILogger<InsuranceService> logger)
        {
            this.productDataService = productDataService;
            this.surchargeService = surchargeService;
            this.logger = logger;
        }

        public async Task<float> CalculateInsurance(ProductInsuranceDto productInsuranceDto)
        {
            var product = await this.productDataService.GetProductDetails(productInsuranceDto.ProductId);
            product.ProductType = await this.productDataService.GetProductTypeDetails(product.ProductTypeId);

            this.logger.LogInformation($"Caclulating Insurance for Product {product.Name} of productId :{productInsuranceDto.ProductId}");

            return CalculateProductInsurance(product);
        }

        public async Task<float> CalculateOrderInsurance(OrderInsuranceDto orderInsuranceDto)
        {
            float insuranceValue = 0;
            bool orderHasDigitalCamera = false;

            foreach (var productId in orderInsuranceDto.ProductIds)
            {
                var product = await this.productDataService.GetProductDetails(productId);
                product.ProductType = await this.productDataService.GetProductTypeDetails(product.ProductTypeId);
                var productTypeSurcharge = this.surchargeService.GetSurchargeRate(product.ProductTypeId);

                this.logger.LogInformation($"Caclulating Insurance for Product {product.Name} of productId :{productId}");

                if (!orderHasDigitalCamera)
                {
                    orderHasDigitalCamera = product.ProductType.Name.Equals("Digital cameras", System.StringComparison.OrdinalIgnoreCase);
                }

                var productInsurance = CalculateProductInsurance(product);
                if (productTypeSurcharge.IsSuccess)
                {
                    this.logger.LogInformation($"Adding surcharge rate : {productTypeSurcharge.SurchargeRate} for Product Type {product.ProductType} of producTypetId :{product.ProductTypeId}");
                    var surchargeValue = (productInsurance * productTypeSurcharge.SurchargeRate) / 100;
                    productInsurance += surchargeValue;
                }

                insuranceValue += productInsurance;
            }

            if (orderHasDigitalCamera)
            {
                this.logger.LogInformation("Adding 500 Euros to Insurance as Order has one or more Digital Cameras");
                insuranceValue += 500;
            }

            return insuranceValue;
        }

        private float CalculateProductInsurance(ProductDto product)
        {
            float productInsurance = 0;

            if (product.ProductType.CanBeInsured)
            {
                if (product.SalesPrice >= 500 && product.SalesPrice < 2000)
                {
                    productInsurance += 1000;
                }
                if (product.SalesPrice >= 2000)
                {
                    productInsurance += 2000;
                }
                if (product.ProductType.Name == "Laptops" || product.ProductType.Name == "Smartphones")
                {
                    productInsurance += 500;
                }
            }
            else
            {
                this.logger.LogInformation($"Not Adding Insurance for {product.Name} of productId :{product.ProductId} as product type cannot be insured");
            }

            return productInsurance;
        }
    }
}
