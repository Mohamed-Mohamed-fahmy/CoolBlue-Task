using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IProductDataService productDataService;
        private readonly ISurchargeService surchargeService;

        public InsuranceService(IProductDataService productDataService, ISurchargeService surchargeService)
        {
            this.productDataService = productDataService;
            this.surchargeService = surchargeService;
        }

        public async Task<float> CalculateInsurance(CalculateInsuranceDto calculateInsuranceDto)
        {
            var product = await this.productDataService.GetProductDetails(calculateInsuranceDto.ProductId);
            product.ProductType = await this.productDataService.GetProductTypeDetails(product.ProductTypeId);


            return CalculateProductInsurance(product);
        }

        public async Task<float> CalculateOrderInsurance(CalculateOrderInsuranceDto calculateOrderInsuranceDto)
        {
            float insuranceValue = 0;
            bool orderHasDigitalCamera = false;

            foreach (var productId in calculateOrderInsuranceDto.ProductIds)
            {
                var product = await this.productDataService.GetProductDetails(productId);
                product.ProductType = await this.productDataService.GetProductTypeDetails(product.ProductTypeId);
                var productTypeSurcharge = this.surchargeService.GetSurchargeRate(product.ProductTypeId);

                if (!orderHasDigitalCamera)
                {
                    orderHasDigitalCamera = product.ProductType.Name.Equals("Digital cameras", System.StringComparison.OrdinalIgnoreCase);
                }

                var productInsurance = CalculateProductInsurance(product);
                if (productTypeSurcharge.IsSuccess)
                {
                    var surchargeValue = (productInsurance * productTypeSurcharge.SurchargeRate) / 100;
                    productInsurance += surchargeValue;
                }

                insuranceValue += productInsurance;
            }

            if (orderHasDigitalCamera)
            {
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

            return productInsurance;
        }
    }
}
