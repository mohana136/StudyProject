using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Handlers.Queries.GetById
{
    public class UpdateUserIdTodo
    {
        public string Title { get; set; }
        public string State { get; set; }
        public DateTime DueDate { get; set; }
        public Boolean OverDue { get; set; }
    }
}
