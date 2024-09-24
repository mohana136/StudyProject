using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Repository
{
    public interface ITodoRepository<Mtodo>
    {
        public Task<Mtodo> GetByID(int id);
        public Task<List<Mtodo>> GetAll();
        public Task<Mtodo> Add(Mtodo item);
        public Task<Mtodo> update(Mtodo item);

        public Task<bool> Delete(int id);

    }
}
