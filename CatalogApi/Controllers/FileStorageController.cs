using CatalogApi.Helpers;
using CatalogApi.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileStorageController: ControllerBase
{
    private readonly IFilmService _filmService;

    public FileStorageController(IFilmService filmService)
    {
        _filmService = filmService;
    }
    
    [Admin]
    [HttpPost("upload")]
    public IActionResult UploadImage(IFormFile file, int id)
    {
        try
        {
            _filmService.UploadImage(file, id);
            return Ok(new {message = $"Image was added!"});
        }
        catch (AppException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }
    
    [Admin]
    [HttpGet("download/{fileId:int}")]
    public IActionResult DownloadImage(int fileId)
    {
        try
        {
            return _filmService.DownloadImage(fileId);
        }
        catch (AppException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }
}