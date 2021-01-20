using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        private User() { }
        public User(string email, string password, string role)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            Role = role;
        }

        [Key]
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; set; }
    }
}