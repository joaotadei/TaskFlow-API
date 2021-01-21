using API.Data;
using API.Dtos;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpPost()]
        public async Task<dynamic> Create([FromBody] CreateToDoItemDto modelDto)
        {
            if (ModelState.IsValid)
                return modelDto;

            var user = await userService.GetByEmail(User.Identity.Name);

            user.AddToDoItem(modelDto.Description, modelDto.Expiration);

            db.Update(user);
            await db.SaveChangesAsync();

            return Ok("Inserido com sucesso");
        }

        [Authorize()]
        [HttpGet()]
        public async Task<List<ToDoItem>> GetAll()
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            return user.ToDoItems;
        }

        [Authorize()]
        [HttpPatch("concluir/{id}")]
        public async Task<dynamic> Finish(Guid id)
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            var item = user.ToDoItems.FirstOrDefault(x => x.Id == id);

            if (item is null)
                return NotFound("Item não encontrado");

            item.Finish();

            db.Update(user);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        [Authorize()]
        [HttpPatch()]
        public async Task<dynamic> Update([FromBody] UpdateToDoItemDto modelDto)
        {
            var toDoItem = await db.ToDoItems.SingleOrDefaultAsync(x => x.Id == modelDto.Id);

            if (toDoItem is null)
                return NotFound();

            toDoItem.UpdateDescriptionAndExpiration(modelDto.Description, modelDto.Expiration);

            await db.SaveChangesAsync();

            return Ok(toDoItem);
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