using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        public LoginController()
        {

        }

        //[HttpPost]
        //public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserAccountDto userAuthenticate)
        //{
        //    var user = await db.Users.SingleOrDefaultAsync(u => u.Email == userAuthenticate.Email && u.Password == userAuthenticate.Password);

        //    if (user == null)
        //        return NotFound(new { message = "Usuário ou senha inválidos" });

        //    // Gera o Token
        //    //var token = TokenService.GenerateToken(user);

        //    // Oculta a senha
        //    //user.Password = "";

        //    // Retorna os dados
        //    return new
        //    {
        //        user = user,
        //        //token = token
        //    };
        //}
    }
}
