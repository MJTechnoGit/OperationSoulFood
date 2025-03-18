using OperationSoulFood.Services.EmailAPI.Models.Dto;

namespace OperationSoulFood.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
