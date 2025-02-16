using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities
{
    public class Log
    {
        private Log() { }
        public Log(string description)
        {
            Description = description;
            Created = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Created { get; private set; }
    }
}