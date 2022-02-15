using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Rating;
using CatalogApi.Models.Users;
using Microsoft.EntityFrameworkCore;

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
    }
    
    public class FilmService : IFilmService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        
        public FilmService(CatalogContext context, IMapper mapper, IRatingService ratingService)
        {
            _context = context;
            _mapper = mapper;
            _ratingService = ratingService;
        }

        public void Create(CreateModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            if (_context.Films.Any(x => x.Title == film.Title))
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
            Film film;
            if ((film = GetById(id)) == null)
                throw new AppException("Incorrect Id = " + id);
            _context.Comments.Include(c =>c.User).ToList();
            _context.SaveChanges();
            var y = this.GetById(id).Comments;
            var z = GetRatingByFilmId(id);
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

        private double GetRatingByFilmId(int id)
        {
            return _ratingService.GetRatingByFilmTitle(GetById(id).Title);
        }
    }
}