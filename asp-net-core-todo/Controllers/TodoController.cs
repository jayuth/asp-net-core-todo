﻿using System;
using System.Threading.Tasks;
using asp_net_core_todo.Models;
using asp_net_core_todo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_todo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
		// set a variable typed ITodoItemService so we can use it in our action methods
		private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        // constructor to instanitiate a TodoController object that matches the properties in ITodoItemService
        public TodoController(ITodoItemService todoItemService,
            UserManager<ApplicationUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }
        // Index action method - display all to-do items 
        public async Task<IActionResult> Index()
        {
            // look up the full user details in the database through GetUserAsync() method
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // the controller calls into the service layer to save the new to-do item.
            // AddItemAsync method will return true of false value.
            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);

            if (!successful)
            {
                return BadRequest(new { error = "Could not add item." } );
            }

            return RedirectToAction("Index");
		}

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            // when passing a parameter that includes a field call 'id' (from a hidden element in index.), 
            // there is no need for model and model binding/validation
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            }

            // item that is checked will disappear as the IsDone property of that item is true now.
            return RedirectToAction("Index");
        }
	}
}
