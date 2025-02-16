using API.Services;
using Dominio.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly TokenService tokenService;
        private readonly UserService userService;
        private readonly AccountService accountService;
        public LoginController(TokenService tokenService, UserService userService, AccountService accountService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
            this.accountService = accountService;
        }

        /// <summary>
        /// Efetuar login
        /// </summary>
        /// <param name="userAuthenticate"></param>
        /// <remarks>Usuário admin padrão - login: "admin@admin.com" password: "123456". Retorna o usuário logado junto ao token de autenticação</remarks>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<dynamic> Authenticate([FromBody] UserAccountDto userAuthenticate)
        {
            try
            {
                var user = await userService.GetByEmailAndPassword(userAuthenticate.Email, userAuthenticate.Password);

                if (user == null)
                    return BadRequest(new { message = "Usuário ou senha inválidos" });

                var token = tokenService.GenerateToken(user);

                user.CleanPassword();

                var userDto = new
                {
                    user = user,
                    token = token
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
