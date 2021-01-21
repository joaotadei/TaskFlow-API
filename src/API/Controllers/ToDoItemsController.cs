using API.Data;
using API.Dtos;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace API.Controllers
{
    [ApiController]
    [Route("itensAfazer")]
    public class ToDoItemsController : Controller
    {
        private readonly Context db;
        private readonly UserService userService;
        private readonly ToDoItemService toDoItemService;
        public ToDoItemsController(Context db, UserService userService, ToDoItemService toDoItemService)
        {
            this.db = db;
            this.userService = userService;
            this.toDoItemService = toDoItemService;
        }

        /// <summary>
        /// Lista os itens a fazer do usuário logado.
        /// </summary>
        [Authorize(Roles = AccountHelper.DefaultUserRole)]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Index()
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            var items = user.ToDoItems.Select(x => new ToDoItemDto(x));

            return Ok(items);
        }

        /// <summary>
        /// Criar um novo item a fazer
        /// </summary>
        /// <param name="modelDto"></param>
        [Authorize(Roles = AccountHelper.DefaultUserRole)]
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Create([FromBody] CreateToDoItemDto modelDto)
        {
            if (!ModelState.IsValid)
                return modelDto;

            var item = await toDoItemService.CreateNew(modelDto, User);

            return Ok(item);
        }

        /// <summary>
        /// Concluir um item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = AccountHelper.DefaultUserRole)]
        [HttpPatch("concluir/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Finish(Guid id)
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            var item = user.ToDoItems.FirstOrDefault(x => x.Id == id);

            if (item is null)
                return NotFound("Item não encontrado");

            user.ToDoItems.FirstOrDefault(x => x.Id == id).Finish();

            await db.SaveChangesAsync();

            return Ok(new ToDoItemDto(item));
        }

        /// <summary>
        /// Atualizar a descrição e data de vencimento de um item
        /// </summary>
        /// <remarks>Um item já finalizado não poderar mais ser alterado</remarks>
        /// <param name="modelDto"></param>
        [Authorize(Roles = AccountHelper.DefaultUserRole)]
        [HttpPatch()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Update([FromBody] UpdateToDoItemDto modelDto)
        {
            var user = await userService.GetByEmail(User.Identity.Name);

            var item = user.ToDoItems.FirstOrDefault(x => x.Id == modelDto.Id);

            if (item is null)
                return NotFound("Item não encontrado");

            if (item.Finished.HasValue)
                return BadRequest("Item concluído, não pode ser editado");

            item.UpdateDescriptionAndExpiration(modelDto.Description, modelDto.Expiration);

            await db.SaveChangesAsync();

            return Ok(new ToDoItemDto(item));
        }

        /// <summary>
        /// Deletar um item
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Apenas o admin pode deletar um item</remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountHelper.AdminUserRole)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Delete(Guid id)
        {
            var item = await db.ToDoItems.FindAsync(id);

            if (item is null)
                return NotFound("Item não encontrado");

            db.Remove(item);
            await db.SaveChangesAsync();

            return Ok("Item removido");
        }

        /// <summary>
        /// Lista todos os itens criados pelos usuarios
        /// </summary>
        /// <param name="page"></param>
        /// <remarks>Apenas o admin pode listar</remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountHelper.AdminUserRole)]
        [HttpGet("listarTodos/{page}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> GetAll(int? page)
        {
            page = (page ?? 1);

            var items = await db.ToDoItems
                .Include(x => x.User)
                .ToPagedListAsync(page, 5);

            return Ok(items.Select(x => new ToDoItemDto(x)).ToList());
        }

        /// <summary>
        /// Buscar em itens atrasados
        /// </summary>
        /// <param name="filtro"></param>
        /// <remarks>Apenas o admin pode listar</remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountHelper.AdminUserRole)]
        [HttpGet("filtrarAtrasados/{filtro}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> GetAllDelayed(string filtro = "")
        {
            filtro = filtro.ToLower();

            var items = await db.ToDoItems
                .Include(x => x.User)
                .Where(x => x.User.Email.ToLower().Contains(filtro) ||
                            x.Description.ToLower().Contains(filtro) ||
                            x.Creation.ToShortDateString().Contains(filtro) ||
                            x.Expiration.ToShortDateString().Contains(filtro))
                .ToListAsync();

            return Ok(items.Where(x => x.Dalayed).Select(x => new ToDoItemDto(x)).ToList());
        }
    }
}