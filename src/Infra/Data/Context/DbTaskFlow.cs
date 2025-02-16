using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class DbTaskFlow : DbContext
    {
        public DbTaskFlow(DbContextOptions<DbTaskFlow> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
    }
}