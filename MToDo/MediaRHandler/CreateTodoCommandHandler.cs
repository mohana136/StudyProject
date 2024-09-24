using MediatR;
using MToDo.MediatRRequests;
using MToDo.Repository;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MToDo.MediatRRequests.MediaRRequests;

namespace MToDo.MediaRHandlers
{
    public record CreateTodoCommand(Mtodo Todo) : IRequest<Mtodo>;
    public record GetTodoByIdQuery(int Id) : IRequest<Mtodo>;
    public class TestCommandHandler : IRequestHandler<TestCommand, string>
    {
        public Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Processed: {request.Input}");
        }
    }
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Mtodo>
    {
        private readonly ICommandService _apiClient;
        public CreateTodoCommandHandler(ICommandService apiClient )
        { 
            _apiClient = apiClient;
        }

        public async Task<Mtodo> Handle(CreateTodoCommand request, CancellationToken cancellationToken )
        {
            var createdTodo = _apiClient.PostToDo(request.Todo);
            return await createdTodo;
        }
    }

    //public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Mtodo>
    //{
    //    private readonly ICommandService _apiClient;

    //    public GetTodoByIdQueryHandler(ICommandService apiclient)
    //    {
    //        _apiClient = apiclient; 
    //    }
    //     public async Task<Mtodo> Handle(GetTodoByIdQuery request , CancellationToken cancellationToken)
    //    {
    //        var todo = _apiClient.GetUserById(request.Id);
    //        return await todo;
    //    }
    //}

    
}
