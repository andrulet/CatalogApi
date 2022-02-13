using Microsoft.AspNetCore.Mvc;
using CatalogApi.Services;
using CatalogApi.Helpers;
using CatalogApi.Models.Users;
using System.Linq;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate(AuthenticateRequest authenticateRequest)
        {
            var response = _userService.Authenticate(authenticateRequest, IpAdress());

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }
        
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            try
            {
                // register user
                var token = _userService.Register(model, IpAdress());
                return Ok(new {token});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout(RevokeTokenRequest model)
        {
            try
            {
                // logout user
                var token = model.Token;
                _userService.RevokeToken(token, IpAdress());
                return Ok(new { message = "Come to us again"});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        private string IpAdress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}