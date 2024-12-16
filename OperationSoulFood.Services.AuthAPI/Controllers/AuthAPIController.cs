using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OperationSoulFood.Services.AuthAPI.Models.Dto;
using OperationSoulFood.Services.AuthAPI.Services.IServices;
using System.Net.NetworkInformation;

namespace OperationSoulFood.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {

        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new();            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            
            var status = await _authService.Register(model);
            if (!string.IsNullOrEmpty(status))
            {
                // An error has occurred..
                _response.IsSuccess = false;
                _response.Message = status; // Get the error message in this situation

                return BadRequest(_response);
            }
            
            // Everything is ok at this point.
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";


                return BadRequest(_response);
            }

            _response.Result = loginResponse;

            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";

                return BadRequest(_response);
            }            

            return Ok(_response);
        }
    }
}
