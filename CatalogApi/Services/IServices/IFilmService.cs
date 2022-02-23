using System.Collections.Generic;
using CatalogApi.Entities;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CatalogApi.Services.IServices;

public interface IFilmService
{
    void Create(CreateModelFilm film);

    FilmInfoResponse GetInfoFilmById(int id);

    Film GetById(int id);
        
    void Delete(int id);

    Film Edit(int id, EditModelFilm film);
    IEnumerable<CommentFilmResponse> GetComments(int id);
        
    void SetRating(SetRatingOnFilm request);

    void UploadImage(IFormFile file, int idFilm);
        
    IActionResult DownloadImage(int idFilm);
    IEnumerable<FilmInfoResponse> SearchByKey(string keyQuery, string valueQuery);
    IEnumerable<FilmInfoResponse> SearchByFilter(Dictionary<string, StringValues> query);
}