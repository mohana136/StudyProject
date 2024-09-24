using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MToDo.Todo;
using MToDo.Repository;

namespace MToDo.Handlers.Queries.GetAll
{

    // Query request definition to get all user lists
    public record GetAllUserListsRequest : IRequest<List<UserDto>>;

    // Handler for processing the request to get all user lists
    public class GetAllHandlerQueriesHandler :IRequestHandler<GetAllUserListsRequest, List<UserDto>>
    {
        private readonly ITodoRepository<Mtodo> _todoRepository;
        public GetAllHandlerQueriesHandler(ITodoRepository<Mtodo> todoRepository)
        {
            _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        public async Task<List<UserDto>> Handle(GetAllUserListsRequest request, CancellationToken cancellationToken)
        {
            // Fetch all todo items from the repository
            var todos = await _todoRepository.GetAll();

            // Map Mtodo objects to UserDto objects
            var UserDto = todos
                                .Select
                                  (u => new UserDto
                                  {
                                     ID = u.ID,
                                     Title = u.Title,
                                     State = u.State,
                                     DueDate = u.DueDate,
                                     OverDue = u.OverDue
                                 }).ToList();
             return UserDto;
        }
    }
}
