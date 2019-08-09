using System;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_todo.Data;
using asp_net_core_todo.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_net_core_todo.Services
{
    public class TodoItemService : ITodoItemService
    {
        // dependency injection - ApplicationDbContext is getting injected
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            // access all the to-do items from the Items property in the DbSet Items
            var items = await _context.Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();
            return items;
        }
    }
}
