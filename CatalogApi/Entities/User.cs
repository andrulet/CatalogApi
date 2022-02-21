using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CatalogApi.Entities
{    
    public class User
    {        
        public int Id { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public DateTime BirthDay { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        
        [JsonIgnore]
        public bool IsAdmin { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();

    }
}
