using System.Collections.Generic;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;
using CatalogApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [Admin]
        [Authorize]
        [HttpPost("create")]
        public IActionResult Create(CreateModelFilm model)
        {
            try
            {
                // create film
                _filmService.Create(model);
                return Ok(new {message = $"Film {model.Title} was added!"});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [Admin]
        [Authorize]
        [HttpPut("{id:int}/edit")]
        public IActionResult Edit(int id, EditModelFilm model)
        {
            try
            {
                // edit film
                _filmService.Edit(id, model);
                return Ok(new {message = $"Film {model.Title} was changed!"});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [Admin]
        [Authorize]
        [HttpDelete("{id:int}/delete")]
        public ActionResult<Film> Delete(int id)
        {
            try
            {
                // edit film
                _filmService.Delete(id);
                return Ok(new {message = $"Film was delete!"});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{filmId:int}/comments")]
        public ActionResult<IList<CommentFilmResponse>> GetComments(int filmId)
        {
            try
            {
                // get comments
                return Ok(_filmService.GetComments(filmId));
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("set_score")]
        public IActionResult SetScoreOnFilmByUser(SetRatingOnFilm request)
        {
            try
            {
                // set rating
                _filmService.SetRating(request);
                return Ok(new {message = $"Set mark {request.ValueRating} "});
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{filmId:int}info")]
        public object GetInfoAboutFilm(int filmId)
        {
            try
            {
                // get info about film
                return _filmService.GetInfoFilmById(filmId);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}