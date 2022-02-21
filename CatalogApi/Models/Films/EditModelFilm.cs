using System;
using System.ComponentModel.DataAnnotations;
using CatalogApi.Entities;

namespace CatalogApi.Models.Films
{
    public class EditModelFilm
    {
        public string _category;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreateFilm { get; set; }
        
        [EnumDataType(typeof(Category))]
        public string Category
        {
            get => _category;
            set => _category = value;
        }
    }
}