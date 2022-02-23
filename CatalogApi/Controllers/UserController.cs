using Microsoft.AspNetCore.Mvc;
using CatalogApi.Helpers;
using AutoMapper;
using CatalogApi.Models.Users;
using CatalogApi.Services.IServices;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        
        [Admin]
        [HttpGet("{id:int}/info")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserResponse>(user); 
            return Ok(model);
        }
    }
}