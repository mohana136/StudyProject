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
    
   
    public record GetAllUserListsRequest : IRequest<List<UserDto>>;

    public class GetAllHandlerQueriesHandler :IRequestHandler<GetAllUserListsRequest, List<UserDto>>
    {
        private readonly ITodoRepository<Mtodo> _todoRepository;
        public GetAllHandlerQueriesHandler(ITodoRepository<Mtodo> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<List<UserDto>> Handle(GetAllUserListsRequest request, CancellationToken cancellationToken)
        {
            var mtodos = await _todoRepository.GetAll();
            var filteredTodos = mtodos
                                .Select
                                  (u => new UserDto
                                  {
                                     ID = u.ID,
                                     Title = u.Title,
                                     State = u.State,
                                     DueDate = u.DueDate,
                                     OverDue = u.OverDue
                                 }).ToList();
             return filteredTodos;
        }
    }
}
