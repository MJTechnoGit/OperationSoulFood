using OperationSoulFood.Web.Models;

namespace OperationSoulFood.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductsAsync(ProductDto product);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto product);
        Task<ResponseDto?> DeleteProductsAsync(int id);
    }
}
