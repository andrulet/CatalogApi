using FileStorage;
using CatalogApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileStorageController: ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public FileStorageController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
    
    [Authorize]
    [HttpPost("upload")]
    public IActionResult UploadImage(int id, byte[] array)
    {
        try
        {
            _fileStorageService.Upload(array, id);
            return Ok(new {message = $"Image was added!"});
        }
        catch (AppException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }
}