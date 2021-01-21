using API.Data;
using API.Dtos;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService
    {
        private readonly Context db;
        public AccountService(Context db)
        {
            this.db = db;
            CreateAdmin();
        }

        public async Task<User> CreateUser(UserAccountDto modelDto)
        {
            var newUser = new User(modelDto.Email, modelDto.Password, AccountHelper.DefaultUserRole);

            db.Add(newUser);
            await db.SaveChangesAsync();

            return newUser;
        }

        public bool AlreadyExisting(string email)
        {
            var alreadyExisting = db.Users.Any(x => x.Email == email);

            return alreadyExisting;
        }

        private void CreateAdmin()
        {
            var adminName = "admin@admin.com";
            var defaultPassword = "123456";
            
            if (!AlreadyExisting(adminName))
            {
                var admin = new User(adminName, defaultPassword, AccountHelper.AdminUserRole);

                db.Add(admin);
                db.SaveChanges();
            }
        }
    }
}