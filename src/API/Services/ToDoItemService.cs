using Dominio.Entities;
using Dominio.Models.Dtos;
using Infra.Data.Context;

namespace API.Services
{
    public class ToDoItemService
    {
        private readonly DbTaskFlow db;
        private readonly UserService userService;
        public ToDoItemService(DbTaskFlow db, UserService userService)
        {
            this.db = db;
            this.userService = userService;
        }

        public async Task<ToDoItemDto> CreateNew(CreateToDoItemDto modelDto, string currentUserEmail)
        {
            var user = await userService.GetByEmail(currentUserEmail);

            var newItem = new ToDoItem(modelDto.Description, modelDto.Expiration);
            
            user.AddToDoItem(newItem);

            await db.SaveChangesAsync();

            return new ToDoItemDto(newItem);
        }
    }
}