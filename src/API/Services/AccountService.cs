using API.Helpers;
using Dominio.Entities;
using Dominio.Models.Dtos;
using Infra.Data.Context;

namespace API.Services
{
    public class AccountService
    {
        private readonly DbTaskFlow db;
        public AccountService(DbTaskFlow db)
        {
            this.db = db;
            CreateAdmin();
        }

        public async Task<User> CreateUser(UserAccountDto modelDto)
        {
            var newUser = new User(modelDto.Email, modelDto.Password, AccountConstants.DefaultUserRole);

            db.Add(newUser);
            await db.SaveChangesAsync();

            return newUser;
        }

        public bool AlreadyExisting(string email)
        {
            var alreadyExisting = db.Users.Any(user => user.Email == email);

            return alreadyExisting;
        }

        private void CreateAdmin()
        {
            var adminName = "admin@admin.com";
            var defaultPassword = "123456";
            
            if (!AlreadyExisting(adminName))
            {
                var admin = new User(adminName, defaultPassword, AccountConstants.AdminUserRole);

                db.Add(admin);
                db.SaveChanges();
            }
        }
    }
}