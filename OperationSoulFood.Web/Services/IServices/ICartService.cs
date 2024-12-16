using OperationSoulFood.Web.Models;

namespace OperationSoulFood.Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponsAsync(CartDto cartDto);
        Task<ResponseDto?> EmailCart(CartDto cartDto);
    }       
}
