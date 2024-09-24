using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
using Serilog;
using MToDo.Data;
using MToDo.Repository;
using Serilog.Context;
using Microsoft.EntityFrameworkCore.Update;
using MediatR;
using static MToDo.MediatRRequests.MediaRRequests;
using MToDo.MediaRHandlers;
using MToDo.Handlers.Queries.GetById;
using MToDo.Handlers.Queries.GetAll;

namespace MToDo.Todo
{
    public class CommandService1 : ICommandService
    {
        ITodoRepository<Mtodo> _todoRepository;
       
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        

        

        public async Task<Mtodo> CreatetodoAsync(Mtodo todo)
        {
            var createdTodo = await _mediator.Send(new CreateTodoCommand(todo));
            return createdTodo;
        }

        public async Task<Mtodo> GetTodoByIdAsync(int id)
        {
            var todo = await _mediator.Send(new GetTodoByIdQuery(id));
            return todo;
        }

        //public async Task<Mtodo> GetUserById(int id)
        //{
        //    Mtodo userId =   _todoRepository.GetByID(id);
        //    Log.Information("userId {uID}", userId.Title);
        //    return userId;
        //}

        public async Task<List<Mtodo>> GetToDo()
        {
            //reading connection string
            //var cs = _config.GetValue<string>("ConnectionStrings:DefaultConnection");            
            //Log.Information("Connection String {cs}", cs);

            List<Mtodo> results =await _todoRepository.GetAll();
            foreach(var res in results)
            {
                var currentTime = DateTime.Now;
                Log.Information("list of todo {title} ,{date} , {overdueflag} , {currenttime}", res.Title,res.DueDate.ToString(),res.OverDue ,currentTime );
            }
            return results;
        }

        public async Task<Mtodo> PostToDo(Mtodo mtodo)
        {
            Mtodo todo =await _todoRepository.Add(mtodo);
            Log.Information("Created item {todo}", todo.Title);
            
            return todo;
        }

        //public Mtodo  UpdateToDo(Mtodo mtodo)
        //{
        //    Mtodo  results  = _todoRepository.update(mtodo);
        //    return results;
        //}

        
    }

    public interface ICommandService
    {


        Task<List<Mtodo>> GetToDo();
        Task<Mtodo> PostToDo(Mtodo mtodo);

        //Task<Mtodo>  GetUserById(int id);

        //Mtodo UpdateToDo(Mtodo mtodo);
    }
}
