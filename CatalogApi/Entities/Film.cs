using System;
using System.Collections.Generic;

namespace CatalogApi.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public Сategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreateFilm { get; set; }
        public List<Comment> Comments { get; set; }
    }
    
    
}