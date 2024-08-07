using Insurance.Api.Models;

namespace Insurance.Api.Interfaces
{
    public interface IBusinessRulesService
    {
        /// <summary>
        /// Get the product details
        /// </summary>
        /// <param name="productTypeID">the product type identifier</param>
        /// <returns></returns>
        ProductTypeDto GetProductTypeDetails(int productTypeID);

        /// <summary>
        /// Get the product type details
        /// </summary>
        /// <param name="productID">the product identifier</param>
        /// <returns></returns>
        ProductDto GetProductDetails(int productID);
    }
}
