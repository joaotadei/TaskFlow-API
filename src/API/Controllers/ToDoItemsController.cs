using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("ToDoItems")]
    public class ToDoItemsController : Controller
    {
        private readonly Context db;
        public ToDoItemsController(Context db)
        {
            this.db = db;
        }

        [HttpGet("GetAll")]
        public async Task<List<ToDoItem>> GetAll()
        {
            var toDoItens = await db.ToDoItems.ToListAsync();

            return toDoItens;
        }

        [HttpGet("Create")]
        public async Task<ToDoItem> Create()
        {
            var toDoItem = new ToDoItem("fazer a api");

            db.Add(toDoItem);
            await db.SaveChangesAsync();

            return toDoItem;
        }
    }
}
