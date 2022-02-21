using System.Linq;
using CatalogApi.Helpers;
using CatalogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }
        
        [HttpGet("all/search")]
        public ActionResult Search()
        {
            try
            {
                // get comments
                var keyQuery = HttpContext.Request.Query.Keys.FirstOrDefault()?.ToLowerInvariant();
                if (string.IsNullOrEmpty(keyQuery))
                    throw new AppException($"{keyQuery} is null");
                var valueQuery = HttpContext.Request.Query[keyQuery];
                return Ok(_filmService.SearchByKey(keyQuery, valueQuery));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }
        
        [HttpGet("all/filter")]
        public ActionResult SearchByFilter()
        {
            try
            {
                // get comments
                var Query = HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value);
                return Ok(_filmService.SearchByFilter(Query));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}

