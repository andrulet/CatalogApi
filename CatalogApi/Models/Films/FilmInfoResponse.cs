using System;
using CatalogApi.Entities;

namespace CatalogApi.Models.Films
{
    public class FilmInfoResponse
    {
        private double _score;
        public int Id { get; set; }
        public Сategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearCreateFilm { get; set; }

        public double Score
        {
            get => Math.Round(_score, 2);
            set => _score = value;
        }

        public FilmInfoResponse(Film film, double score)
        {
            Id = film.Id;
            Category = film.Category;
            Title = film.Title;
            Description = film.Description;
            YearCreateFilm = film.DateCreateFilm.Year;
            Score = score;
        }
    }
}