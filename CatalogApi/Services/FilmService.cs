using System;
using FileStorage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogApi.Services
{
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
    }
    
    public class FilmService : IFilmService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _path;
        
        public FilmService(CatalogContext context, IConfiguration configuration, IMapper mapper, IRatingService ratingService, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _ratingService = ratingService;
            _fileStorageService = fileStorageService;
            _path = configuration.GetSection("ConnectionStrings:PathToImages").Value;
        }

        public void Create(CreateModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            if (_context.Films.Any(x => x.Title == model.Title))
                throw new AppException("Film'" + film.Title + "' is already taken");
            _context.Films.Add(film);
            _context.SaveChanges();
        }
        public FilmInfoResponse GetInfoFilmById(int id)
        {
            return new FilmInfoResponse(GetById(id), GetRatingByFilmId(id));
        }
        
        public void Delete(int id)
        {
            _context.Films.Remove(GetById(id));
            _context.SaveChanges();
        }

        public Film Edit(int id, EditModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            film.Id = id;
            if (_context.Films.Any(x => x.Title == film.Title && x.Id != film.Id))
                throw new AppException("Film'" + film.Title + "' is already taken");
            
            var f =GetById(film.Id);
            _context.Films.Remove(f);
            f = film;
            _context.Films.Update(f);
            _context.SaveChanges();
            return f;
        }

        public IEnumerable<CommentFilmResponse> GetComments(int id)
        {
            if (GetById(id) == null)
                throw new AppException("Incorrect Id = " + id);
            var comments = _context.Comments.Include(c =>c.User).ToList();
            var y = this.GetById(id).Comments;
            return y.Select(x => new CommentFilmResponse(x));
        }

        public void SetRating(SetRatingOnFilm request)
        {
            _ratingService.SetRating(request);
        }
        
        public Film GetById(int id)
        {
            Film film;
            if ((film = _context.Films.Find(id)) == null)
                throw new AppException("Incorrect Id = " + id);
            return film;
        }

        public void UploadImage(IFormFile file, int idFilm)
        {
            var film = GetById(idFilm);
            var pathFile = string.IsNullOrEmpty(film.Path) ? _path + DateTime.Now.Ticks + "." + file.ContentType.Split('/').Last() : film.Path;
            _fileStorageService.Upload(file, pathFile);
            film.Path = pathFile;
            _context.SaveChanges();
        }

        public IActionResult DownloadImage(int idFilm)
        {
            var film = GetById(idFilm);
            var pathFile = film.Path;
            if (string.IsNullOrEmpty(pathFile))
                throw new AppException("Path is null");
            var stream = _fileStorageService.Download(pathFile);
            return new FileStreamResult(stream, "image/" + film.Path.Split(".").Last());
        }

        private double GetRatingByFilmId(int id)
        {
            return _ratingService.GetRatingByFilmTitle(id);
        }
    }
}