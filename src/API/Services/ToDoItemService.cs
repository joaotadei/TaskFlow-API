using API.Data;
using API.Dtos;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Services
{
    public class ToDoItemService
    {
        private readonly Context db;
        private readonly UserService userService;
        public ToDoItemService(Context db, UserService userService)
        {
            this.db = db;
            this.userService = userService;
        }

        public async Task<dynamic> CreateNew(CreateToDoItemDto modelDto, ClaimsPrincipal userIdentity)
        {
            var user = await userService.GetByEmail(userIdentity.Identity.Name);

            var newItem = new ToDoItem(modelDto.Description, modelDto.Expiration);
            
            user.AddToDoItem(newItem);

            await db.SaveChangesAsync();

            return new ToDoItemDto(newItem);
        }
    }
}