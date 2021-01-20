using API.Data;
using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("Autenticacao")]
    public class AccountsController : Controller
    {
        private readonly Context db;
        private readonly AccountService accountService;
        public AccountsController(Context db, AccountService accountService)
        {
            this.db = db;
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserAccountDto userAuthenticate)
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Email == userAuthenticate.Email && u.Password == userAuthenticate.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            //var token = TokenService.GenerateToken(user);

            // Oculta a senha
            //user.Password = "";

            // Retorna os dados
            return new
            {
                user = user,
                //token = token
            };
        }

        [HttpPost("criar")]
        public async Task<dynamic> Create([FromBody] UserAccountDto createUserAccount)
        {
            if (!ModelState.IsValid)
                return createUserAccount;

            var newUser = await accountService.CreateUser(createUserAccount);

            return newUser;
        }
    }
}