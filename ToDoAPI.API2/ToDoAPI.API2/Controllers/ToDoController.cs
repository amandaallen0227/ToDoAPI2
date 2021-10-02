using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF; //added to connect to the data layer
using ToDoAPI.API.Models; //access to the DTO's - what are DTO's?
using System.Web.Http.Cors; 

namespace ToDoAPI.API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController 
    {
        //object that connects to the DB
        ToDoEntities db = new ToDoEntities();

        //api/ToDo/
        public IHttpActionResult GetToDos()
        {
            //Create a list of resources from the db
            List<ToDoViewModel> todos = db.TodoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                //assign the params of the ToDos from the db to a data transfer obj
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                   CategoryId = (int)t.CategoryId,
                   Name = t.Category.Name,
                   Description = t.Category.Description
                }

            }).ToList<ToDoViewModel>();

            //Check on the results and handle accordingly below
            if (todos.Count == 0)
            {
                return NotFound();
            }

            return Ok(todos);


        }//end GetToDos


        
        //api/ToDos/id
        public IHttpActionResult GetToDo(int id)
        {
            ToDoViewModel todo = db.TodoItems.Include("Category").Where(t => t.TodoId == id).Select(t => new ToDoViewModel()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                Category = new CategoryViewModel()
                {
                    CategoryId = (int)t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).FirstOrDefault();

            if (todo == null)
            {
                return NotFound();

            }

            return Ok(todo);
        }//end GetToDo

        //PostToDo
        //Http api/todos
        public IHttpActionResult PostToDo(ToDoViewModel todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data"); //scopeless if statement.

            TodoItem newToDo = new TodoItem()
            {
                TodoId = todo.TodoId,
                Action = todo.Action,
                Done = todo.Done,
                CategoryId = todo.CategoryId
            };

            db.TodoItems.Add(newToDo);
            db.SaveChanges();
            return Ok(newToDo);
          
        }//end postToDo

        //PutToDo
        //HttpPut
        public IHttpActionResult PutToDo(ToDoViewModel todo)




        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }//end if

            TodoItem existingToDo = db.TodoItems.Where(t => t.TodoId == todo.TodoId).FirstOrDefault();

            if (existingToDo != null)
            {
                existingToDo.TodoId = todo.TodoId;
                existingToDo.Action = todo.Action;
                existingToDo.Done = todo.Done;
                existingToDo.CategoryId = todo.CategoryId;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }//end PutToDo


        //DeleteToDo
        //api/todos (httpdelete)
        public IHttpActionResult DeleteToDo(int id)
        {
            TodoItem todo = db.TodoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (todo != null)
            {
                db.TodoItems.Remove(todo);
                db.SaveChanges();
                return Ok();
            }//end of
            else
            {
                return NotFound();
            }



        }//deletetodo

        //Dispose
        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                db.Dispose(); //close that db connection!
            }


            base.Dispose(disposing);    
        }


    }
}
