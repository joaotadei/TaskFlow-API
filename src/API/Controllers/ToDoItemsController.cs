using API.Helpers;
using API.Services;
using Dominio.Models.Dtos;
using Infra.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace API.Controllers
{
    [ApiController]
    [Route("itensAfazer")]
    public class ToDoItemsController : Controller
    {
        private readonly DbTaskFlow db;
        private readonly UserService userService;
        private readonly ToDoItemService toDoItemService;
        public ToDoItemsController(DbTaskFlow db, UserService userService, ToDoItemService toDoItemService)
        {
            this.db = db;
            this.userService = userService;
            this.toDoItemService = toDoItemService;
        }

        /// <summary>
        /// Lista os itens a fazer do usuário logado.
        /// </summary>
        [Authorize(Roles = AccountConstants.DefaultUserRole)]
        [HttpGet()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Index()
        {
            try
            {
                var user = await userService.GetByEmail(User.Identity.Name);

                var todoItem = user.ToDoItems.Select(itens => new ToDoItemDto(itens));

                return Ok(todoItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Criar um novo item a fazer
        /// </summary>
        /// <param name="modelDto"></param>
        [Authorize(Roles = AccountConstants.DefaultUserRole)]
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Create([FromBody] CreateToDoItemDto modelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return modelDto;

                var todoItem = await toDoItemService.CreateNew(modelDto, User.Identity.Name);

                return Ok(todoItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Concluir um item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = AccountConstants.DefaultUserRole)]
        [HttpPatch("concluir/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Finish(Guid id)
        {
            try
            {
                var user = await userService.GetByEmail(User.Identity.Name);

                var todoItem = user.ToDoItems.FirstOrDefault(items => items.Id == id);

                if (todoItem is null)
                    return BadRequest("Item não encontrado");

                user.ToDoItems.FirstOrDefault(items => items.Id == id).Finish();

                await db.SaveChangesAsync();

                return Ok(new ToDoItemDto(todoItem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar a descrição e data de vencimento de um item
        /// </summary>
        /// <remarks>Um item já finalizado não poderar mais ser alterado</remarks>
        /// <param name="modelDto"></param>
        [Authorize(Roles = AccountConstants.DefaultUserRole)]
        [HttpPatch()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Update([FromBody] UpdateToDoItemDto modelDto)
        {
            try
            {
                var user = await userService.GetByEmail(User.Identity.Name);

                var todoItem = user.ToDoItems.FirstOrDefault(item => item.Id == modelDto.Id);

                if (todoItem is null)
                    return NotFound("Item não encontrado");

                if (todoItem.Finished.HasValue)
                    return BadRequest("Item concluído, não pode ser editado");

                todoItem.UpdateDescriptionAndExpiration(modelDto.Description, modelDto.Expiration);

                await db.SaveChangesAsync();

                return Ok(new ToDoItemDto(todoItem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletar um item (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Apenas o admin pode deletar um item </remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountConstants.AdminUserRole)]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Delete(Guid id)
        {
            try
            {
                var todoItem = await db.ToDoItems.FindAsync(id);

                if (todoItem is null)
                    return NotFound("Item não encontrado");

                db.Remove(todoItem);
                await db.SaveChangesAsync();

                return Ok("Item removido");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os itens criados pelos usuarios (Admin)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <remarks>Apenas o admin pode listar</remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountConstants.AdminUserRole)]
        [HttpGet("listarTodos/{page}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> GetAll(int? pageNumber)
        {
            try
            {
                pageNumber = (pageNumber ?? 1);

                var todoItems = await db.ToDoItems
                    .Include(x => x.User)
                    .ToPagedListAsync(pageNumber, 5);

                return Ok(todoItems.Select(item => new ToDoItemDto(item)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Buscar em itens atrasados (Admin)
        /// </summary>
        /// <param name="filtro"></param>
        /// <remarks>Apenas o admin pode listar</remarks>
        /// <returns></returns>
        [Authorize(Roles = AccountConstants.AdminUserRole)]
        [HttpGet("buscarItensAtrasadosPorDescricaoOuData/{filtro}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> FindByDescriptionDataDelayed(string filtro = "")
        {
            try
            {
                filtro = filtro.ToLower();

                var todoItems = await db.ToDoItems
                    .Include(x => x.User)
                    .Where(x => x.User.Email.ToLower().Contains(filtro) ||
                                x.Description.ToLower().Contains(filtro) ||
                                x.Creation.ToShortDateString().Contains(filtro) ||
                                x.Expiration.ToShortDateString().Contains(filtro))
                    .ToListAsync();

                return Ok(todoItems.Where(x => x.Dalayed).Select(x => new ToDoItemDto(x)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}