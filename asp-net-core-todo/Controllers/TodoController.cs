using System;
using System.Threading.Tasks;
using asp_net_core_todo.Models;
using asp_net_core_todo.Services;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_todo.Controllers
{
    public class TodoController : Controller
    {
		// set a variable typed ITodoItemService so we can use it in our action methods
		private readonly ITodoItemService _todoItemService;

		// constructor to instanitiate a TodoController object that matches the properties in ITodoItemService
		public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        // Index action method - display all tasks 
        public async Task<IActionResult> Index()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();

            // bind a list of items returned from GetIncompleteItemsAsync() method to TodoViewModel() 
            var model = new TodoViewModel()
            {
                Items = items
            };

            return View(model);
        }

		// AddItem action method to handle /Todo/AddItem route
		[ValidateAntiForgeryToken]
        // this method will carry a parameter (a TodoItem object with a Title property) returned from a submit form from a partial view, AddItemPartial.cshtml
        // this method will also perform a 'model binding'   
        public async Task<IActionResult> AddItem(TodoItem newItem)
		{
			if (!ModelState.IsValid)
			{
                return RedirectToAction("Index");
			}

            // the controller calls into the service to save the new to-do item.
            // AddItemAsync method will return true of false value.
            var successful = await _todoItemService.AddItemAsync(newItem);

            if (!successful)
            {
                return BadRequest(new { error = "Could not add item." } );
            }

            return RedirectToAction("Index");
		}
	}
}
