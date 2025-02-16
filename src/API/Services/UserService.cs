using Dominio.Entities;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService
    {
        private readonly DbTaskFlow db;
        public UserService(DbTaskFlow db)
        {
            this.db = db;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await db.Users
                .Include(user => user.ToDoItems)
                .SingleOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await db.Users
                .Include(user => user.ToDoItems)
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Email == email && user.Password == password);
        }
    }
}