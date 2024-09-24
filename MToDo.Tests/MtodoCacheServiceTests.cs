using FakeItEasy;
using MToDo.Repository;
using MToDo.Todo;
using MToDo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MToDo;
using Serilog;

public class MtodoCacheServiceTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MtodoCacheService _mtodoCacheService;
    private readonly ITodoRepository<Mtodo> _todoRepository;
    //private readonly ICommandService _todoApiClient;

    public MtodoCacheServiceTests()
    {
        // Fake the ITodoApiClient instance
        _todoRepository = A.Fake<ITodoRepository<Mtodo>>();

        // Configure a real ServiceProvider with the ITodoApiClient added as a singleton
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_todoRepository);
        _serviceProvider = serviceCollection.BuildServiceProvider();

        // Mock TodoConfig with a time interval
        var todoConfig = Options.Create(new TodoConfig { TimeInterval = 60 });

        // Instantiate the service with real ServiceProvider and config
        _mtodoCacheService = new MtodoCacheService(_serviceProvider, todoConfig);
    }

    [Fact]
    public void DoWork_ShouldUpdateOverdueTasks()
    {
        // Arrange: Set up a list of fake todo items
        var todoList = new List<Mtodo>
        {
            new Mtodo {  DueDate = DateTime.Now.AddDays(-1), OverDue = false },
            new Mtodo { DueDate = DateTime.Now.AddDays(-2), OverDue = false }
        };

        // Fake the behavior of GetToDo to return the list of overdue items
        A.CallTo(() => _todoRepository.GetAll())
            .Returns(todoList);

        // Act: Execute the DoWork method
        _mtodoCacheService.DoWork();  
        
    }

    [Fact]
    public async Task StartAsync_ShouldStartTimer_AndLogMessage()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act
        await _mtodoCacheService.StartAsync(cancellationToken);

        // Assert
        A.CallTo(() => Log.Information("Timer started")).MustHaveHappenedOnceExactly();
    }
}
