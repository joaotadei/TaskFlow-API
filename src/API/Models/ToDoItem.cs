using System;
using System.Collections.Generic;
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
        public DateTime? Finished { get; private set; }
        public List<Log> UpdatesLogs { get; private set; } = new List<Log>();

        public void UpdateDescriptionAndExpiration(string description, DateTime expiration)
        {
            this.Description = description;
            this.Expiration = expiration;

            LogAction("Updated item");
        }
        public void Finish()
        {
            this.Finished = DateTime.Now;

            LogAction("Finished item");
        }

        private void LogAction(string description)
        {
            this.UpdatesLogs.Add(new Log(description));
        }
    }
}