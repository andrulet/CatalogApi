using System;
using System.Data;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models;
using CatalogApi.Models.Rating;
using CatalogApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CatalogApi.Services
{
    public interface IRatingService
    {
        void SetRating(SetRatingOnFilm request);
        double GetRatingByFilm(int filmid);
    }
    public class RatingService : IRatingService
    {
        private readonly IRepository<Rating> _ratingRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public RatingService(IRepository<Rating> ratingRepository, IMapper mapper, IConfiguration configuration)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        public void SetRating(SetRatingOnFilm request)
        {
            if (_ratingRepository.GetAll().Any(x => x.FilmId == request.FilmId && x.UserId == request.UserId))
                throw new AppException("Mark by you on this film is already done");
            if (request.ValueRating < 0 || request.ValueRating > 10)
                throw new AppException("Mark must be between 1 and 10(include)");
            var rait = _mapper.Map<Rating>(request);
            _ratingRepository.Insert(rait);
            _ratingRepository.Save();
        }

        public double GetRatingByFilm(int filmid)
        {
            double rating;
            var sqlExpression = "Get_Score"; 
 
            using (var connection = new SqlConnection(_configuration.GetConnectionString("CatalogApiDatabase")))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                var id = new SqlParameter
                {
                    ParameterName = "@filmid",
                    Value = filmid
                };
                    
                command.Parameters.Add(id);

                var score = new SqlParameter
                {
                    ParameterName = "@score",
                    SqlDbType = SqlDbType.Float, 
                    Direction = ParameterDirection.Output
                };
                 
                command.Parameters.Add(score);
                command.ExecuteNonQuery();
                rating = (double)command.Parameters["@score"].Value;
            }
            return rating;
        }
    }
}