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

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            // set the rest of the properties for the newItem object
            // the newItem.Title propery has already been set by ASP.NET Core's model binder
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);

            // tell the database to add a new item to Items table
            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await _context.Items
                .Where(x => x.Id == id)
                // return the item if it exists or null
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}
