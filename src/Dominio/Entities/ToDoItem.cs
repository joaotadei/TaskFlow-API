using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Expiration { get; private set; }
        public DateTime Creation { get; private set; }
        public DateTime? Finished { get; private set; }
        public User User { get; private set; }
        public List<Log> UpdatesLogs { get; private set; } = new List<Log>();
        public bool Dalayed { get
            {
                var dalayed = false;

                if (this.Finished.HasValue)
                    return dalayed;

                var expired = this.Expiration < DateTime.Now;
                if (expired)
                    dalayed = true;

                return dalayed;
            }
        }

        public void UpdateDescriptionAndExpiration(string description, DateTime expiration)
        {
            this.Description = description;
            this.Expiration = expiration;

            LogAction($"Updated item by {this.User.Email}");
        }

        public void Finish()
        {
            this.Finished = DateTime.Now;

            LogAction($"Finished item by {this.User.Email}");
        }

        private void LogAction(string description)
        {
            this.UpdatesLogs.Add(new Log(description));
        }
    }
}