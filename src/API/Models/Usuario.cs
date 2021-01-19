using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public IList<ToDoItem> ToDoItems { get; private set; }
    }
}
