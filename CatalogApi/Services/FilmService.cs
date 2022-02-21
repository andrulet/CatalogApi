using System;
using FileStorage;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models;
using CatalogApi.Models.Collections;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;
using CatalogApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

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
        IEnumerable<FilmInfoResponse> SearchByKey(string keyQuery, string valueQuery);
        IEnumerable<FilmInfoResponse> SearchByFilter(Dictionary<string, StringValues> query);
    }
    
    public class FilmService : IFilmService
    {
        private readonly IRepository<Film> _filmRepository;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly IFileStorageService _fileStorageService;
        
        private readonly string _path;
        
        public FilmService(
            IRepository<Film> filmRepository,
            IConfiguration configuration,
            IMapper mapper,
            IRatingService ratingService,
            IFileStorageService fileStorageService)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _ratingService = ratingService;
            _fileStorageService = fileStorageService;
            _path = configuration.GetSection("ConnectionStrings:PathToImages").Value;
        }

        public void Create(CreateModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            if (_filmRepository.GetAll().Any(x => x.Title == model.Title))
                throw new AppException("Film'" + film.Title + "' is already taken");
            _filmRepository.Insert(film);
            _filmRepository.Save();
            SetRating(new SetRatingOnFilm(){FilmId = film.Id});
            _filmRepository.Save();
        }
        public FilmInfoResponse GetInfoFilmById(int id)
        {
            return new FilmInfoResponse(GetById(id), GetRatingByFilmId(id));
        }
        
        public void Delete(int id)
        {
            _filmRepository.Delete(id);
            _filmRepository.Save();
        }

        public Film Edit(int id, EditModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            film.Id = id;
            if (_filmRepository.GetAll().Any(x => x.Title == film.Title && x.Id != film.Id))
                throw new AppException("Film'" + film.Title + "' is already taken");
            
            _filmRepository.Update(film);
            _filmRepository.Save();
            return film;
        }

        public IEnumerable<CommentFilmResponse> GetComments(int id)
        {
            Film film;
            if ((film = GetById(id)) == null)
                throw new AppException("Incorrect Id = " + id);
            _filmRepository.GetAll();
            //_filmRepository(c =>c.User).Load();
            _filmRepository.LoadAllComments();
            var y = GetById(id).Comments;
            return y.Select(x => new CommentFilmResponse(x));
        }

        public void SetRating(SetRatingOnFilm request)
        {
            if (GetById(request.FilmId) == null)
            {
                throw new AppException("There is no such film.");
            }
            _ratingService.SetRating(request);
        }
        
        public Film GetById(int id)
        {
            
            Film film;
            if ((film = _filmRepository.GetById(id)) == null)
                throw new AppException("Incorrect Id = " + id);
            return film;
        }

        public void UploadImage(IFormFile file, int idFilm)
        {
            var film = GetById(idFilm);
            var pathFile = string.IsNullOrEmpty(film.Path) ? _path + DateTime.Now.Ticks + "." + file.ContentType.Split('/').Last() : film.Path;
            _fileStorageService.Upload(file, pathFile);
            film.Path = pathFile;
            _filmRepository.Save();
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

        public double GetRatingByFilmId(int id)
        {
            return _ratingService.GetRatingByFilm(id);
        }

        public IEnumerable<FilmInfoResponse> SearchByKey(string keyQuery, string valueQuery)
        {
            var films = GetAll();
            var keyAndValueSearch = GetNamePropertyAndValue(keyQuery, valueQuery);
            if (keyAndValueSearch == (null,null)) 
                throw new AppException("Incorrect key for searching.");
            return GetFilmsBySearchKeyAndValue(keyAndValueSearch, films);
        }

        public IEnumerable<FilmInfoResponse> SearchByFilter(Dictionary<string, StringValues> query)
        {
            var films = GetAll();
            var parameters = SetParams(query);
            var selectFilms = films.Where(f =>
                f.Score <= parameters.MaxScore && f.Score >= parameters.MinScore &&
                f.YearCreateFilm.Year <= parameters.MaxYear && f.YearCreateFilm.Year >= parameters.MinYear
            ).ToList();

            var result = new List<FilmInfoResponse>();
            foreach (var x in parameters.Categories)
            {
                result.AddRange(selectFilms.Where(y => y.Category == Enum.GetName(x)));
            }

            return result;
        }

        private ParametersForSearching SetParams(Dictionary<string, StringValues> query)
        {
            var parameters = new ParametersForSearching();
            foreach (var pair in query)
            {
                GetParamByKey(pair, ref parameters);
            }

            return parameters;
        }

        private IEnumerable<FilmInfoResponse> GetAll()
        {
            var films = _filmRepository.GetAll();
            return films.Select(x => GetInfoFilmById(x.Id));
        }

        private (PropertyInfo,dynamic) GetNamePropertyAndValue(string keyQuery, string valueQuery)
        {
            Type type = typeof(FilmInfoResponse);
            switch (keyQuery)
            {
                case "title":
                    return (type.GetProperty("Title"), valueQuery);
                case "category":
                    return (type.GetProperty("Category"), valueQuery);
                case "score":
                    return (type.GetProperty("Score"),Convert.ToDouble(valueQuery));
                default:
                    return (null, null);
            }
        }

        private IEnumerable<FilmInfoResponse> GetFilmsBySearchKeyAndValue((PropertyInfo, dynamic) keyAndValueSearch,
            IEnumerable<FilmInfoResponse> films)
        {
            if (keyAndValueSearch.Item1.Name == "Title")
                return films.Where(f => f.Title.ToLowerInvariant().Contains(((string)keyAndValueSearch.Item2).ToLowerInvariant()));
            if (keyAndValueSearch.Item1.Name == "Category")
                return films.Where(f => f.Category == keyAndValueSearch.Item2);
            if (keyAndValueSearch.Item1.Name == "Score")
                return films.Where(f => f.Score == keyAndValueSearch.Item2);
            return null;
        }

        private void GetParamByKey(KeyValuePair<string, StringValues> pair, ref ParametersForSearching parametres)
        {
            switch (pair.Key.ToLowerInvariant())
            {
                case "minyear":
                    parametres.MinYear = Convert.ToInt32(pair.Value);
                    break;
                case "maxyear":
                    parametres.MaxYear = Convert.ToInt32(pair.Value);
                    break;
                case "minscore":
                    parametres.MinScore = Convert.ToDouble(pair.Value);
                    break;
                case "maxscore":
                    parametres.MaxScore = Convert.ToDouble(pair.Value);
                    break;
                case "categories":
                    parametres.Categories = pair.Value.ToString().Split(",").Select(x =>Enum.Parse<Category>(x));
                    break;
                
            }
        }
    }
}