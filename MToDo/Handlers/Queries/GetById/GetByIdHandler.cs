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
    public record GetUserDetailsCommand(int ID): IRequest<Mtodo>;
    public class GetUserDetailsCommandHandler : IRequestHandler<GetUserDetailsCommand , Mtodo>
    {
        private readonly ITodoRepository<Mtodo> _todoRepository;

        public GetUserDetailsCommandHandler(ITodoRepository<Mtodo> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Mtodo> Handle(GetUserDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Mtodo getUser = new Mtodo();
                if (request != null)
                {
                     getUser = await _todoRepository.GetByID(request.ID);
                    return getUser;
                }
                return getUser;
            }
            catch (Exception ex)
            {
                return new Mtodo();
            }
        }

    }
}
