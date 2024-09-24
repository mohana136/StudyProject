using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using MToDo.Repository;
using MToDo.MediaRHandlers;
using MediatR;
using static MToDo.MediatRRequests.MediaRRequests;
using MToDo.MediatRRequests;
using static MToDo.Handlers.Queries.GetById.GetByIdHandler.CreateCommandHandler;
using MToDo.Handlers.Queries.GetById.GetByIdHandler;

namespace MToDo.Tests
{
    public class CreateTodoCommandHandlerTests
    {
        private readonly ICommandService _fakeTodoApiClient;
        private readonly ITodoRepository<Mtodo> _fakeTodoRepository;
        private readonly IRequestHandler<CreateTodoCommand, Mtodo> _handlerCmd;
        private readonly IRequestHandler<GetTodoByIdQuery, Mtodo> _handleQuery;
        public CreateTodoCommandHandlerTests()
        {
            // create a fake instance of ITodoRepository<Mtodo>
            _fakeTodoRepository = A.Fake<ITodoRepository<Mtodo>>();
            _fakeTodoApiClient = A.Fake<ICommandService>();
            _handlerCmd = new CreateTodoCommandHandler(_fakeTodoApiClient);
            _handleQuery = new GetTodoByIdQueryHandler(_fakeTodoApiClient);
        }

        [Fact]
        public async Task Handle_ShouldCreateTodoSuccessfully()
        {
            //Arrange: Setup a fake Todo item to be added
            var fakeTodo = new Mtodo {ID=1084, Title = "Test", State = "Done", DueDate = DateTime.Now };
            A.CallTo(() => _fakeTodoApiClient.PostToDo(fakeTodo)).Returns(Task.FromResult(fakeTodo));

            //Act : call the posttodo method
            var result = await _handlerCmd.Handle(new CreateTodoCommand(fakeTodo), CancellationToken.None);
            //Assert:check if the add Todo item is returned correctly
            Assert.NotNull(result);
            Assert.Equal("Test" , result.Title);
            Assert.Equal("Done", result.State);

        }
        //[Fact]
        //public async Task Handle_ShouldGetUserByIdSuccessfully()
        //{
        //    //Arrange: setup a fake todo item 
        //    var toDo = new Mtodo { ID = 1, Title = "Test", State = "Pending" };
        //    A.CallTo(() => _fakeTodoApiClient.GetUserById(1)).Returns(Task.FromResult(toDo));
        //    var byIdRequest = new GetTodoByIdQuery(1);
        //    //Act : Call the GetUserById method
        //    var result = await _handleQuery.Handle(byIdRequest, CancellationToken.None);    

        //    //Assert : Check if the returned result matches the expected result
        //    Assert.NotNull(result);
        //    Assert.Equal("Test", result.Title);
        //    Assert.Equal("Pending", result.State.ToString());
        //}

        [Fact]
        public async Task Handle_shouldReturnProcessedString()
        {
            //Arrange 
            var command = new CreateCommandHandler { , "Pending", DateTime.Parse(args[2]), false };
            var fakeComHandler = new TestCommandHandler();

            //Act
            var result =await fakeComHandler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal("New Task Created with Id: 1099", result);
        }

    }
}
