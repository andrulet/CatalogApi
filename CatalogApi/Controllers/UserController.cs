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
        [Authorize]
        [HttpGet("{id:int}/info")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user); 
            return Ok(model);
        }
    }
}