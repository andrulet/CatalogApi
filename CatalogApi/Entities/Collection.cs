using System.Collections.Generic;

namespace CatalogApi.Entities
{
    public class Collection
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public bool IsPrivate { get; set; }
        
        public int? UserId { get; set; }
        
        public virtual User User { get; set; }

        public virtual ICollection<Film> Films { get; set; } = new List<Film>();
    }
}