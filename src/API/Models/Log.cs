using System;

namespace API.Models
{
    public class Log
    {
        private Log() { }
        public Log(string description)
        {
            Description = description;
            Created = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Created { get; private set; }
    }
}
