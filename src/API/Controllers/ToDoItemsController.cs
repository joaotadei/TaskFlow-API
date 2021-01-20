using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("itensAfazer")]
    public class ToDoItemsController : Controller
    {
        private readonly Context db;
        public ToDoItemsController(Context db)
        {
            this.db = db;
        }

        [HttpGet()]
        public async Task<List<ToDoItem>> GetAll()
        {
            var toDoItens = await db.ToDoItems.ToListAsync();

            return toDoItens;
        }

        [HttpPost()]
        public async Task<dynamic> Create([FromBody] CreateToDoItemDto modelDto)
        {
            var toDoItem = new ToDoItem(modelDto.Description, modelDto.Expiration);

            db.Add(toDoItem);
            await db.SaveChangesAsync();

            return toDoItem;
        }

        [HttpGet("/{toDoItemId}")]
        public async Task<dynamic> Details(Guid toDoItemId)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == toDoItemId);

            if (toDoItem is null)
                return NotFound();

            return toDoItem;
        }

        [HttpPut()]
        public async Task<dynamic> Update([FromBody] UpdateToDoItemDto modelDto)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == modelDto.Id);

            if (toDoItem is null)
                return NotFound();

            return toDoItem;
        }
    }
}