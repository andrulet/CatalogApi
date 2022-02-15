using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CatalogApi.Services;
using CatalogApi.Helpers;
using AutoMapper;
using CatalogApi.Models.Users;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        
        [Admin]
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserResponse>>(users); 
            return Ok(model);
        }
    }
}