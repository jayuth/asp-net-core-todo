using System;
using System.ComponentModel.DataAnnotations;

namespace asp_net_core_todo.Models
{
    public class TodoItem
    {
        public string UserId { get; set; }

        // data the database will need to store for each to-do item
        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTimeOffset? DueAt { get; set; }
    }
}
