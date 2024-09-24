using MediatR;
using MToDo.Handlers.Queries.GetAll;
using MToDo.Repository;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Handlers.Queries.GetById.GetByIdHandler
{
    public class CreateCommandHandler
    {
        public record CreateUserDetailsCommand(string Title, string State, DateTime DueDate, Boolean OverDue) : IRequest<Mtodo>;

        public class CreateDetailsCommandHandler : IRequestHandler<CreateUserDetailsCommand, Mtodo>
        {
            private readonly ITodoRepository<Mtodo> _todoRepository;
            public CreateDetailsCommandHandler(ITodoRepository<Mtodo> todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task<Mtodo> Handle(CreateUserDetailsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request is not null)
                    {
                        var id = Guid.NewGuid();
                        var newtodo =  await _todoRepository.Add(new Mtodo
                        {
                            //ID = id,
                            Title = request.Title,
                            State = request.State,
                            DueDate = request.DueDate,
                            OverDue = request.OverDue
                        });
                        return newtodo;
                    }
                    else
                    {
                        // Return a default value or handle the null request case
                        throw new ArgumentNullException(nameof(request), "Request cannot be null");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }

        }
    }
}
