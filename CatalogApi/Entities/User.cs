using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        
        public List<Comment> Comments { get; set; } = new List<Comment>();

        [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
