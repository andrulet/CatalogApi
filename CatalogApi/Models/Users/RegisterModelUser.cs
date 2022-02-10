using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogApi.Models.Users
{
    public class RegisterModelUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}