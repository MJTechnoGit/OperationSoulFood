using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OperationSoulFood.Web.Models;
using OperationSoulFood.Web.Services.IServices;
using OperationSoulFood.Web.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OperationSoulFood.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider) 
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();

            return View(loginRequestDto);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {

            ResponseDto responseDto = await _authService.LoginAsync(model);            


            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                responseDto.Message = "Login Successful";

                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                TempData["success"] = responseDto.Message;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError("CustomError", responseDto.Message);

                TempData["error"] = responseDto.Message;
                return View(model);
            }     
        }       


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text =SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{Text =SD.RoleCustomer, Value=SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {

            ResponseDto result = await _authService.RegisterAsync(model);
            ResponseDto assignedRole;
           

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = SD.RoleCustomer;
                }

                assignedRole = await _authService.AssignRoleAsync(model);  
                if (assignedRole != null && assignedRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text =SD.RoleAdmin, Value=SD.RoleAdmin },
                new SelectListItem{Text =SD.RoleCustomer, Value=SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Clear the cookies and token.
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();

            return RedirectToAction("Index","Home");
        }


        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // Add some claims to the token.
            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value),
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value),
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value),
                new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value),  // This claims is needed for .Net Identity.
                new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value)                          // Also needed for .Net Identity.
            };

            // Then add the claims to the user identity to tell asp.net core that the user is actually logged in.

            identity.AddClaims(claimList);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
