using System;

namespace CatalogApi.Entities
{
    public class Film
    {
        public int Id { get; set; }
        
        public Сategory Category { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }

        public DateTime DateCreate { get; set; }
        
        public string NameImage { get; set; }
        
        public byte[] Image { get; set; }
    }
}