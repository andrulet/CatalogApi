using Microsoft.AspNetCore.Mvc;
using CatalogApi.Services;
using CatalogApi.Helpers;
using CatalogApi.Entities;
using AutoMapper;
using CatalogApi.Models.Users;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public IActionResult Authenticate(AuthenticateRequest authenticateModel)
        {
            var response = _userService.Authenticate(authenticateModel);

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            // map model to entity
            try
            {
                // register user
                var response = _userService.Register(registerRequest);
                return Ok(new
                {
                    message = $"Thank you, {response.FirstName}, for registration!",
                    response
                });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}