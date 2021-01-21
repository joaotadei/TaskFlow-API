using API.Data;
using API.Dtos;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserService userService;
        public ToDoItemsController(Context db, UserService userService)
        {
            this.db = db;
            this.userService = userService;
        }

        [Authorize()]
        [HttpGet()]
        public async Task<List<ToDoItem>> GetAll()
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            return user.ToDoItems;
        }

        [Authorize()]
        [HttpPost()]
        public async Task<dynamic> Create([FromBody] CreateToDoItemDto modelDto)
        {
            if (ModelState.IsValid)
                return modelDto;

            var user = await userService.GetByEmail(User.Identity.Name);

            user.AddToDoItem(modelDto.Description, modelDto.Expiration);

            db.Update(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [Authorize()]
        [HttpGet("{id}")]
        public async Task<dynamic> GetById(Guid id)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id);

            if (toDoItem is null)
                return NotFound();

            return toDoItem;
        }

        [Authorize()]
        [HttpPut()]
        public async Task<dynamic> Update([FromBody] UpdateToDoItemDto modelDto)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == modelDto.Id);

            if (toDoItem is null)
                return NotFound();

            toDoItem.UpdateDescriptionAndExpiration(modelDto.Description, modelDto.Expiration);

            await db.SaveChangesAsync();

            return toDoItem;
        }

        [Authorize()]
        [HttpDelete("{id}")]
        public async Task<dynamic> Delete(Guid id)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == id);

            if (toDoItem is null)
                return NotFound();

            db.Remove(toDoItem);
            await db.SaveChangesAsync();

            return Ok("Item removido");
        }
    }
}