using Dominio.Entities;

namespace Dominio.Models.Dtos
{
    public class ToDoItemDto
    {
        public ToDoItemDto() { }
        public ToDoItemDto(ToDoItem toDoItem)
        {
            Id = toDoItem.Id;
            Description = toDoItem.Description;
            Expiration = toDoItem.Expiration;
            Creation = toDoItem.Creation;
            Finished = toDoItem.Finished;
            User = toDoItem.User?.Email;
            UpdatesLogs = toDoItem.UpdatesLogs;
            Dalayed = toDoItem.Dalayed;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? Finished { get; set; }
        public string User { get; set; }
        public List<Log> UpdatesLogs { get; set; } = new List<Log>();
        public bool Dalayed { get; set; }
    }
}