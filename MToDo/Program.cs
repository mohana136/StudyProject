using Azure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using MToDo.Data;
using MToDo.Handlers.Queries.GetAll;
using MToDo.Handlers.Queries.GetById.GetByIdHandler;
using MToDo.Repository;
using MToDo.Services;
using MToDo.Todo;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Text;
using static MToDo.Handlers.Queries.GetById.GetByIdHandler.CreateCommandHandler;
using static MToDo.Handlers.Queries.GetById.GetByIdHandler.DeleteUserCommandHandler;
using static MToDo.Handlers.Queries.GetById.GetByIdHandler.UpdateUserCommandHandler;


namespace MToDo
{
    class Program
    {
        private static ServiceProvider serviceProvider;
        private static async Task Main(string[] args)
        {
            try
            {
                // Setup Dependency Injection and build service provider
                serviceProvider = ConfigureServices();
                                
                var mediator = serviceProvider.GetRequiredService<IMediator>();
                var hostedService = serviceProvider.GetRequiredService<IHostedService>();
                string command = "";
                                
                // Handle command-line arguments
                if (args.Length == 0)
                {
                    Console.WriteLine("Please provide a valid command.");
                    Console.ReadLine();
                }
                else
                {
                    command = args[0].ToLower();
                }
                command = args[0].ToLower();

                //Handle command-line arguments            

                var response = await HandleCommand(command, args, mediator);

                await hostedService.StartAsync(CancellationToken.None);
                //Log the response
                Console.WriteLine(response);
                
                //stop the background service 
                await hostedService.StopAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Log the exception to the console
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }            

            finally
            {
                // Keep the console open to show output
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }

        // Method to handle CRUD operations based on user input
        private static async Task<object> HandleCommand(string commandlineargument,  string[] args, IMediator mediator )
        {
            string outputString;
            switch (commandlineargument)
            {
                case "add":
                    if(args.Length < 2)
                        return "Usage: mtodo add \"task description\" <duedate>";

                    string taskDescription = args[1];
                   
                    var newTask = await mediator.Send(new CreateUserDetailsCommand(taskDescription, "Pending", DateTime.Parse(args[2]), false));
                    return $"New Task Created with Id: {newTask.ID}";

                case "getall":
                    var allTasks =  await mediator.Send(new GetAllUserListsRequest());
                    if (allTasks ==null || !allTasks.Any())
                    {
                        return "No tasks found";
                    }
                    var resultbuilder = new StringBuilder();
                    resultbuilder.AppendLine("All Tasks:");
                    foreach(var task in allTasks)
                    {
                        resultbuilder.AppendLine( $"ID:{task.ID} , Title:{task.Title}, State: {task.State}, Date: {task.DueDate}, OverDue: {task.OverDue}");
                    }
                    return resultbuilder.ToString();


                case "update":
                    if (args.Length < 2)
                        return "Usage: mtodo update \"ID\" \"task description\" <duedate>";

                    string Id= args[1];
                    string taskTitle = args[2];

                    return await mediator.Send(new UpdateUserDetailsCommand(Int32.Parse(Id), taskTitle, "Pending", DateTime.Now, false));

                case "delete":
                    if (args.Length < 1)
                        return "Usage:mtodo delete \"ID\" ";

                    string ID = args[1];

                    return await mediator.Send(new DeleteUserDetailsCommand(Int32.Parse(ID)));

                case "get":
                    if (args.Length < 1)
                        return "Usuage:mtodo get \"ID\" ";

                    string UserID = args[1];

                    var getUser = await mediator.Send(new GetUserDetailsCommand(Int32.Parse(UserID)));
                    
                    if (getUser == null)
                    {
                        return $"Task with ID {UserID} not found.";
                    }

                    return $"Task details: \n" + 
                           $"ID: { getUser.ID} \n" +
                           $"Title: {getUser.Title} \n" +
                           $"Status: {getUser.State} \n" +
                           $"DueDate : {getUser.DueDate}";

                case "hostedservice":                   
                    Console.WriteLine("MtodoCacheService DoWork method has been triggered.");
                    return "Cache service started.";

                default:
                    throw new ArgumentException("Invalid command");
            }
        }
                
        // Configures services using Microsoft.Extensions.DependencyInjection
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Load configuration and logging setup
            var configuration = BuildConfiguration();
            ConfigureLogging(configuration);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                           
            // Register services
                services.AddDbContext<ToDoDbContext>();
                services.AddScoped<ITodoRepository<Mtodo>, TodoRepository>();
                services.AddHostedService<MtodoCacheService>();
                services.Configure<TodoConfig>(configuration.GetSection(nameof(TodoConfig)));            
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            }
            return services.BuildServiceProvider();
        }

        // Builds the application configuration
        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        // Configures logging using Serilog
        private static void ConfigureLogging(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/demo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
