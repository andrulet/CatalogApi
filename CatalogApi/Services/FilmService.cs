using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Films;
using CatalogApi.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services
{
    public interface IFilmService
    {
        void Create(CreateModelFilm film);
        
        IEnumerable<Film> GetAll();

        Film GetById(int id);

        Rating SetScore();

        void Delete(int id);

        Film Edit(int id, EditModelFilm film);
        IEnumerable<CommentFilmResponse> GetComments(int id);
    }
    
    public class FilmService : IFilmService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;
        
        public FilmService(CatalogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(CreateModelFilm model)
        {
            var film = _mapper.Map<Film>(model);
            if (_context.Films.Any(x => x.Title == film.Title))
                throw new AppException("Film'" + film.Title + "' is already taken");
            _context.Films.Add(film);
            _context.SaveChanges();
        }

        public IEnumerable<Film> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Film GetById(int id)
        {
            Film film;
            if ((film = _context.Films.Find(id)) == null)
                throw new AppException("Incorrect Id = " + id);
            return film;
        }

        public Rating SetScore()
        {
            throw new System.NotImplementedException();
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
            _context.Entry(film).Collection(t=>t.Comments).Load();
            return _mapper.Map<IList<CommentFilmResponse>>(film.Comments);
        }
    }
}