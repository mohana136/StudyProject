using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MToDo.Data;
using MToDo.Todo;
using Serilog.Context;

namespace MToDo.Repository
{
    public class TodoRepository : ITodoRepository<Mtodo>
    {
        ToDoDbContext _context;

        public TodoRepository(ToDoDbContext context)
        {
                _context = context;
        }
        public async Task<Mtodo> GetByID(int id)
        {
            var todo = await _context.Mtodos.FirstOrDefaultAsync(x => x.ID == id);
                        
            //LogContext.PushProperty("userId = ", id);
            return todo;
        }
        public async Task<List<Mtodo>> GetAll()
        {
            return await _context.Mtodos.ToListAsync();
        }
        public async Task<Mtodo> Add(Mtodo mtodo)
        {
            _context.Mtodos.Add(mtodo);
            await _context.SaveChangesAsync();
            return mtodo;
        }

        public async Task<bool> Delete(int id)
        {
            var mtodo = await _context.Mtodos.FirstOrDefaultAsync(x => x.ID == id);

            if(mtodo !=null)
            {
                _context.Mtodos.Remove(mtodo);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }    
        
        public async Task<Mtodo> update(Mtodo item)
        {
            Mtodo mtodo = _context.Mtodos.Where(x=> x.ID == item.ID).FirstOrDefault();
            if(mtodo != null)
            {
                _context.Entry(mtodo).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
            return item;
        }
    }
}
