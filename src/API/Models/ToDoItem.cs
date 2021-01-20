﻿using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ToDoItem
    {
        private ToDoItem() { }
        public ToDoItem(string description, DateTime expiration)
        {
            Id = Guid.NewGuid();
            Description = description;
            Expiration = expiration;
            Creation = DateTime.Now;
        }

        [Key]
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Expiration { get; private set; }
        public DateTime Creation { get; private set; }
    }
}