using MToDo.Handlers.Queries.GetById.GetByIdHandler;
using FluentAssertions;
using MToDo.Repository;
using MToDo.Todo;
using FakeItEasy;

namespace MToDo.Tests
{
    

    public class CreateDetailsCommandHandlerTests
    {
        private readonly ITodoRepository<Mtodo> _todoRepository;
        private readonly CreateCommandHandler.CreateDetailsCommandHandler _handler;

        public CreateDetailsCommandHandlerTests()
        {
            //setup fakeiteasy
            _todoRepository = A.Fake<ITodoRepository<Mtodo>>();
            _handler = new CreateCommandHandler.CreateDetailsCommandHandler(_todoRepository);
        }
        [Fact]
        public async Task Handle_Should_Call_Add_Method_And_Return_New_Todo()
        {
            var command = new CreateCommandHandler.CreateUserDetailsCommand(
            Title: "New Task",
            State: "Pending",
            DueDate: DateTime.UtcNow.AddDays(5),
            OverDue: false
        );

            var expectedToDo = new Mtodo
            {
                Title = command.Title,
                State = command.State,
                DueDate = command.DueDate,
                OverDue = command.OverDue
            };

            // Simulate the Add method to return the expected object
            A.CallTo(() => _todoRepository.Add(A<Mtodo>.Ignored))
                .Returns(Task.FromResult(expectedToDo));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => _todoRepository.Add(A<Mtodo>.That.Matches(t =>
            t.Title == command.Title &&
            t.State == command.State &&
            t.DueDate == command.DueDate &&
            t.OverDue == command.OverDue)))
            .MustHaveHappenedOnceExactly();

            result.Should().BeEquivalentTo(expectedToDo);
        }


    }
}
