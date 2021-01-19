using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ToDoItem
    {
        private ToDoItem() { }
        public ToDoItem(string description)
        {
            Description = description;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
    }
}