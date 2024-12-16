using OperationSoulFood.Services.AuthAPI.Models;

namespace OperationSoulFood.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser application, IEnumerable<string> roles);
    }
}
