using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MToDo.Repository;
using MToDo.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.VisualBasic;
using Microsoft.Extensions.DependencyInjection;

namespace MToDo.Services
{
    public class MtodoCacheService : IHostedService
    {
        private readonly IServiceProvider _services;

        private int _number = 0;
        private readonly System.Timers.Timer _timer;
        private readonly TodoConfig _todoConfig;
        private List<Mtodo> _todoList;

        public MtodoCacheService(IServiceProvider services,IOptions<TodoConfig> todoConfig)
        {
            _services = services;
            _todoConfig = todoConfig.Value;
            _timer = new(TimeSpan.FromSeconds(_todoConfig.TimeInterval));
            _timer.Enabled = true;
            _timer.AutoReset = true;           
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Elapsed += OnTimerExecute;
            _timer.Start();
            Log.Information("Timer started");
            return Task.CompletedTask;            
        }

        public  void OnTimerExecute(object? sender, System.Timers.ElapsedEventArgs e)
        {
             DoWork();
        }

        /// <summary>
        /// UpdateDueDate
        /// </summary>
        /// <returns></returns>
        public async Task DoWork()
        {
            
            using var scope = _services.CreateScope();
            //var service = scope.ServiceProvider.GetRequiredService<ICommandService>();

            //Repository service to access the database
            var todoRepository = scope.ServiceProvider.GetRequiredService<ITodoRepository<Mtodo>>();

            List<Mtodo> mtodos = await todoRepository.GetAll();
            Console.WriteLine($"Number of tasks retrieved: {mtodos.Count}");


            List<Mtodo> overDueTasks = mtodos.Where(d => d.DueDate <= DateTime.Now && d.OverDue == false).ToList();
            
            //update overdue flag
            foreach (var task in overDueTasks)
            {
                task.OverDue = true;
                todoRepository.update(task); //Save the updated task to the database                
                Console.WriteLine($"ID: {task.ID}, Title: {task.Title}, State: {task.State}, DueDate: {task.DueDate}, OverDue: {task.OverDue}");

                //Console.WriteLine("its overdue");
            }
            _todoList = overDueTasks;
        }

        public  Task StopAsync(CancellationToken cancellationToken)
        {
             Log.Information("Timed fosted service is stopping");
            _timer?.Stop();
             DoWork();  
             return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();


    }
}
