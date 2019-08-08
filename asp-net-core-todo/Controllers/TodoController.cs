using System;
using System.Threading.Tasks;
using asp_net_core_todo.Models;
using asp_net_core_todo.Services;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_todo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        // constructor
        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        // Action method
        public async Task<IActionResult> Index()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();

            var model = new TodoViewModel()
            {
                Items = items
            };

            return View(model);
        }
    }
}
