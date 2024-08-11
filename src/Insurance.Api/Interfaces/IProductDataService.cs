using Insurance.Api.DTOs;
using System.Threading.Tasks;

namespace Insurance.Api.Interfaces
{
    public interface IProductDataService
    {
        /// <summary>
        /// Get the product details
        /// </summary>
        /// <param name="productTypeID">the product type identifier</param>
        /// <returns></returns>
        Task<ProductTypeDto> GetProductTypeDetails(int productTypeID);

        /// <summary>
        /// Get the product type details
        /// </summary>
        /// <param name="productID">the product identifier</param>
        /// <returns></returns>
        Task<ProductDto> GetProductDetails(int productID);
    }
}
