using System;

namespace API.Models
{
    public class ToDoItem
    {
        private ToDoItem() { }
        public ToDoItem(string description, DateTime expiration)
        {
            Description = description;
            Expiration = expiration;
            Creation = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Creation { get; private set; }
        public DateTime Expiration { get; private set; }
    }
}