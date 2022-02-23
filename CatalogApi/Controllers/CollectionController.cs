using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CatalogApi.Helpers;
using CatalogApi.Models.Collections;
using CatalogApi.Services.IServices;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollectionController : ControllerBase
    {
        private ICollectionService _collectionService;
        
        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateCollection(CollectionCreateRequest request)
        {
            try
            {
                // create Collection
                _collectionService.Create(request);
                return Ok(new { message = $"Your collection was created." });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpDelete("delete/{collectionId:int}")]
        public IActionResult DeleteCollection(int collectionId)
        {
            try
            {
                // create delete collection
                _collectionService.Delete(collectionId);
                return Ok(new { message = $"Your collection was deleted." });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPost("addfilm")]
        public IActionResult AddFilm(FilmAddIntoCollectionRequest response)
        {
            try
            {
                // add film into collection
                _collectionService.AddFilmIntoCollection(response);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpGet("get/{collectionId:int}")]
        public IActionResult GetInfoCollection(int collectionId)
        {
            try
            {
                // get info collection
                var result = _collectionService.GetCollectionInfo(collectionId);
                return Ok(!result.Any() ? new { messsge = "This collection is empty"} : result);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPut("edit/{collectionId:int}")]
        public IActionResult EditCollection(int collectionId, CollectionEditRequest request)
        {
            try
            {
                // edit collection
                return Ok(_collectionService.EditCollection(collectionId, request));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}