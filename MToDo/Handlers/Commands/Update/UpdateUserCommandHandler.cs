using MediatR;
using MToDo.Repository;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Handlers.Queries.GetById.GetByIdHandler
{
    public class UpdateUserCommandHandler
    {
        public record UpdateUserDetailsCommand(int ID,string Title, string State, DateTime DueDate, Boolean OverDue) : IRequest<Mtodo>;

        public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand , Mtodo>
        {
            private readonly ITodoRepository<Mtodo> _todoRepository;
            public UpdateUserDetailsCommandHandler(ITodoRepository<Mtodo> todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task<Mtodo> Handle(UpdateUserDetailsCommand request ,CancellationToken cancellationToken)
            {
                try
                {
                    if (request is not null)
                    {
                        var userDetails = await _todoRepository.update(new Mtodo
                        {
                            ID = request.ID,
                            Title = request.Title,
                            State = request.State,
                            DueDate = request.DueDate,
                            OverDue = request.OverDue
                        });
                       
                        return  userDetails;
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
