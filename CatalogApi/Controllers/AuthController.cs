using System.Collections.Generic;
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
        public IActionResult Authenticate(AuthenticateModel authenticateModel)
        {
            var response = _userService.Authenticate(authenticateModel.Email, authenticateModel.Password);

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
            user.IsAdmin = false;
            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok(new { message = $"Thank you, {user.FirstName}, for registration!" });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}