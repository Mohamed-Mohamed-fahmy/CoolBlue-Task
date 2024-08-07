using Insurance.Api.Interfaces;
using Insurance.Api.Models;

namespace Insurance.Api.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IBusinessRulesService businessRulesService;

        public InsuranceService(IBusinessRulesService businessRulesService)
        {
            this.businessRulesService = businessRulesService;
        }

        public float CalculateInsurance(CalculateInsuranceDto calculateInsuranceDto)
        {
            var product = this.businessRulesService.GetProductDetails(calculateInsuranceDto.ProductId);
            product.ProductType = this.businessRulesService.GetProductTypeDetails(product.ProductTypeId);


            return CalculateProductInsurance(product);
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
