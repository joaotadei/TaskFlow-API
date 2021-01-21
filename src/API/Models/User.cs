using System;
using System.Collections.Generic;
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
        public List<ToDoItem> ToDoItems { get; private set; } = new List<ToDoItem>();

        public void AddToDoItem(ToDoItem item)
        {
            this.ToDoItems.Add(item);
        }
        public void CleanPassword() => this.Password = "";
    }
}