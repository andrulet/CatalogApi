using System;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Comments; 
using CatalogApi.Repositories.CommentRepository;
using CatalogApi.Services.IServices;

namespace CatalogApi.Services
{
    public class CommentService: ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IFilmService _filmService;
        
        public CommentService(ICommentRepository commentRepository, IMapper mapper, IFilmService filmService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _filmService = filmService;
        }
        
        public CommentResponse Create(CreateCommentRequest request)
        {
            var com = _mapper.Map<Comment>(request); 
            com.DateCreate = DateTime.Now;
            com.Film = _filmService.GetById(com.FilmId);
            _commentRepository.Insert(com);
            _commentRepository.Save();
            return _mapper.Map<CommentResponse>(_commentRepository.GetById(com.Id));
        }

        public void Delete(int id)
        {
            if (GetById(id) == null)
                throw new AppException($"Invalid id = {id}");
            _commentRepository.Delete(id);
            _commentRepository.Save();
        }

        public void Edit(int id, EditCommentRequest request)
        {
            if (string.IsNullOrEmpty(request.Content) || _commentRepository.GetById(id).UserId != request.UserId)
                throw new AppException($"Message is Empty");
            var com = GetById(id);
            if (com == null)
                throw new AppException("Incorrect Id");
            com.Content = request.Content;
            com.DateCreate = DateTime.Now;
            _commentRepository.Update(com);
            _commentRepository.Save();
        }

        private Comment GetById(int id)
        {
            return _commentRepository.GetById(id);
        }
    }
}