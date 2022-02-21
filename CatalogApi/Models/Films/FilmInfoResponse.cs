using System;
using System.ComponentModel.DataAnnotations;
using CatalogApi.Entities;

namespace CatalogApi.Models.Films
{
    public class FilmInfoResponse
    {
        public string _category;
        
        private double _score;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime YearCreateFilm { get; set; }
        
        public double Score
        {
            get => Math.Round(_score, 2);
            set => _score = value;
        }
        
        [EnumDataType(typeof(Category))]
        public string Category
        {
            get => _category;
            set => _category = value;
        }

        public FilmInfoResponse(Film film, double score)
        {
            Id = film.Id;
            Category = Enum.GetName(film.Category);
            Title = film.Title;
            Description = film.Description;
            YearCreateFilm = film.DateCreateFilm;
            Score = score;
        }
    }
}