using MediatR;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.MediatRRequests
{
    public class TestCommand : IRequest<string>
    {
        public string Input { get; set; }
    }
    public class MediaRRequests : IRequest<Mtodo>
    {
        //command to create a new Todo item
        

        //Query to fetch a Todo item by Id
        
    }
}
