using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CollectionsController : ControllerBase
{
    private ICollectionService _collectionService;
        
    public CollectionsController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }
    
    [Authorize]
    [HttpGet("all")]
    public IActionResult CreateCollection()
    {
        try
        {
            // create Collection
            var user = new object();
            HttpContext.Items.TryGetValue("User", out user);
            return Ok(_collectionService.GetAll(((User)user).Id));
        }
        catch (AppException ex)
        {
            // return error message if there was an exception
            return BadRequest(new { message = ex.Message });
        }
    }
}