using Microsoft.AspNetCore.Mvc;
using CatalogApi.Services;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private ICommentsService _commentsService;
        
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        
        [Authorize]
        [HttpPost("create")]
        public IActionResult Create(CreateCommentRequest model)
        {
            try
            {
                // create comment
                return Ok(_commentsService.Create(model));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                // delete comment
                _commentsService.Delete(id);
                return Ok(new { message = $"Your comment was deleted." });
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPut("edit/{id:int}")]
        public IActionResult Edit(int id, EditCommentRequest message)
        {
            try
            {
                // edit comment
                _commentsService.Edit(id, message);
                return Ok(new { message = $"Your comment was edited." });
                
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}