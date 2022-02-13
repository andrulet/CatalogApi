using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments;
using CatalogApi.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services
{
    public interface ICommentsService
    {
        CommentResponse Create(CreateCommentRequest request);
        void Delete(int id);
        void Edit(int id, EditCommentRequest request);
        IEnumerable<Comment> GetCommentsByFilmId(int id);
    }
    
    public class CommentsService: ICommentsService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        
        public CommentsService(CatalogContext context, IMapper mapper, IFilmService filmService, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        
        public CommentResponse Create(CreateCommentRequest request)
        {
            var com = _mapper.Map<Comment>(request); 
            com.DateCreate = DateTime.Now;
            com.Film = _context.Films.Find(com.FilmId);
            _context.Comments.Add(com);
            _context.SaveChanges();
            return _mapper.Map<CommentResponse>(_context.Comments.Find(com.Id));
        }

        public void Delete(int id)
        {
            if (_context.Comments.Find(id) == null)
                throw new AppException($"Invalid id = {id}");
            _context.Comments.Remove(GetById(id));
            _context.SaveChanges();
        }

        public void Edit(int id, EditCommentRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
                throw new AppException($"Message is Empty");
            var com = GetById(id);
            if (com == null)
                throw new AppException("Incorrect Id");
            com.Content = request.Content;
            _context.Comments.Update(com);
            _context.SaveChanges();
        }

        public IEnumerable<Comment> GetCommentsByFilmId(int id)
        {
            return _mapper.Map<IEnumerable<Comment>>(_context.Comments.Where(x => x.Id == id)
                .Include(y => y.User.FirstName));
        }

        private Comment GetById(int id)
        {
            return _context.Comments.Find(id);
        }
    }
}