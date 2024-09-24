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
    public class DeleteUserCommandHandler
    {
        public record DeleteUserDetailsCommand(int ID) : IRequest<bool>;

        public class DeleteUserDetailsCommandHandler : IRequestHandler<DeleteUserDetailsCommand, bool>
        {
            private readonly ITodoRepository<Mtodo> _todoRepository;
            public DeleteUserDetailsCommandHandler(ITodoRepository<Mtodo> todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task<bool> Handle(DeleteUserDetailsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request != null)
                    {
                        var isDeleted = await _todoRepository.Delete(request.ID);
                        if(isDeleted)
                        {
                            Console.WriteLine($"Todo item with Id {request.ID} was successfully deleted.");
                        }
                        else
                        {
                            Console.WriteLine($"Todo item with ID {request.ID} was not found or could not be deleted");
                        }
                        return isDeleted;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while deleting Todo item: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
