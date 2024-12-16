using OperationSoulFood.Services.ShoppingCartAPI.Models.Dto;

namespace OperationSoulFood.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
