using MToDo.Handlers.Queries.GetById.GetByIdHandler;
using MToDo.Repository;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace MToDo.Tests
{
    private readonly ITodoRepository<Mtodo> _todoRepository;
    private readonly CreateCommandHandler.CreateDetailsCommandHandler _handler;

    public class CreateDetailsCommandHandlerTests
    {
        //setup fakeiteasy
        //_todoRepository = A.fa
    }
}
