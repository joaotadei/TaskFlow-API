using API.Services;
using Dominio.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("conta")]
    public class AccountsController : Controller
    {
        private readonly AccountService accountService;
        public AccountsController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Criar um usuário.
        /// </summary>
        [AllowAnonymous]
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Create([FromBody] UserAccountDto createUserAccount)
        {
            try
            {
                if (!ModelState.IsValid)
                    return createUserAccount;

                if (accountService.AlreadyExisting(createUserAccount.Email))
                    return Ok("Email já cadastrado");

                var newUser = await accountService.CreateUser(createUserAccount);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}