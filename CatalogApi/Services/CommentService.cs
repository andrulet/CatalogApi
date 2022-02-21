using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models;
using CatalogApi.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services
{
    public interface ICommentService
    {
        CommentResponse Create(CreateCommentRequest request);
        void Delete(int id);
        void Edit(int id, EditCommentRequest request);
    }
    
    public class CommentService: ICommentService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;
        private readonly IFilmService _filmService;
        
        public CommentService(CatalogContext context, IMapper mapper, IFilmService filmService)
        {
            _context = context;
            _mapper = mapper;
            _filmService = filmService;
        }
        
        public CommentResponse Create(CreateCommentRequest request)
        {
            var com = _mapper.Map<Comment>(request); 
            com.DateCreate = DateTime.Now;
            com.Film = _filmService.GetById(com.FilmId);
            _context.Comments.Add(com);
            _context.SaveChanges();
            return _mapper.Map<CommentResponse>(_context.Comments.Find(com.Id));
        }

        public void Delete(int id)
        {
            Comment com;
            if ((com = GetById(id)) == null)
                throw new AppException($"Invalid id = {id}");
            _context.Comments.Remove(com);
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

        private Comment GetById(int id)
        {
            return _context.Comments.Find(id);
        }
    }
}