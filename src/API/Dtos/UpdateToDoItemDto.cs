using System;

namespace API.Dtos
{
    public class UpdateToDoItemDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
    }
}