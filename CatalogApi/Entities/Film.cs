using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CatalogApi.Entities
{
    public class Film
    {
        [JsonIgnore]
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreateFilm { get; set; }
        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [JsonIgnore]
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        [JsonIgnore]
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}