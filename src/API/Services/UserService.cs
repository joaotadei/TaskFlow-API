using API.Data;
using API.Helpers;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserService
    {
        private readonly Context db;
        public UserService(Context db)
        {
            this.db = db;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await db.Users
                .Include(x => x.ToDoItems)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            return await db.Users
                .Include(x => x.ToDoItems)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Email == email && x.Password == password);
        }
    }
}