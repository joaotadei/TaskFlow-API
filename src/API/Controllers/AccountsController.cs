using API.Data;
using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("conta")]
    public class AccountsController : Controller
    {
        private readonly AccountService accountService;
        public AccountsController(Context db, AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost(), AllowAnonymous]
        public async Task<dynamic> Create([FromBody] UserAccountDto createUserAccount)
        {
            if (!ModelState.IsValid)
                return createUserAccount;

            var newUser = await accountService.CreateUser(createUserAccount);

            return newUser;
        }
    }
}