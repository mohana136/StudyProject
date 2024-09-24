using Microsoft.EntityFrameworkCore;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Data
{
    public class ToDoDbContext : DbContext
    {
        //public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Mtodos;Trusted_Connection=True;");
        }

        public DbSet<Mtodo> Mtodos { get; set; } 
    }
}
