using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodoApp.Data
{
    public class Task
    {
        private Task()
        {
        }

        public Task(string description)
        {
            Description = description;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; private set; }

        public bool IsComplete { get; private set; }

        public void MarkComplete()
        {
            IsComplete = true;
        }

        public void MarkIncomplete()
        {
            IsComplete = false;
        }
    }
}
