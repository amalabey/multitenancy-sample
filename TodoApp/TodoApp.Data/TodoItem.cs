using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodoApp.Data
{
    public class TodoItem
    {
        public int Id { get; set; }

        public bool IsComplete { get; set; }

        public string Description { get; set; }
    }
}
