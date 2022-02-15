﻿using System.Data;
using System.Linq;
using AutoMapper;
using CatalogApi.Entities;
using CatalogApi.Helpers;
using CatalogApi.Models.Rating;
using CatalogApi.Models.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Services
{
    public interface IRatingService
    {
        void SetRating(SetRatingOnFilm request);
        double GetRatingByFilmTitle(string titleFilm);
    }
    public class RatingService : IRatingService
    {
        private readonly CatalogContext _context;
        private readonly IMapper _mapper;

        public RatingService(CatalogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void SetRating(SetRatingOnFilm request)
        {
            if (_context.Ratings.Any(x => x.FilmId == request.FilmId && x.UserId == request.UserId))
                throw new AppException("Mark by you on this film is already done");
            if (request.ValueRating < 1 || request.ValueRating > 10)
                throw new AppException("Mark must be between 1 and 10(include)");
            var rait = _mapper.Map<Rating>(request);
            _context.Ratings.Add(rait);
            _context.SaveChanges();
        }

        public double GetRatingByFilmTitle(string titleFilm)
        {
            double rating;
            using(_context)
            {
                /*System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@name", titleFilm);
                var phones = _context.Database.ExecuteSqlRaw("Get_Score @name",param);*/
                
                string sqlExpression = "Get_Score"; 
 
                using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter title = new SqlParameter
                    {
                        ParameterName = "@title",
                        Value = titleFilm
                    };
                    
                    command.Parameters.Add(title);
                    
                    
                    SqlParameter score = new SqlParameter
                    {
                        ParameterName = "@score",
                        SqlDbType = SqlDbType.Float, 
                        Direction = ParameterDirection.Output
                    };
                 
                    command.Parameters.Add(score);

                    var x = command.ExecuteNonQuery();
                    rating = (double)command.Parameters["@score"].Value;
                }
            }

            return rating;
        }
    }
}