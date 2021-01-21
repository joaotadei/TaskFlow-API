using API.Data;
using API.Dtos;
using API.Helpers;
using API.Models;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService
    {
        private readonly Context db;
        private readonly UserService userService;
        public AccountService(Context db)
        {
            this.db = db;
        }

        public async Task<User> CreateUser(UserAccountDto modelDto)
        {
            var newUser = new User(modelDto.Email, modelDto.Password, AccountHelper.DefaultUserRole);

            db.Add(newUser);
            await db.SaveChangesAsync();

            return newUser;
        }


    }
}
