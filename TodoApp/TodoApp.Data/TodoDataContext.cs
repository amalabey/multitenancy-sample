using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data
{
    public class TodoDataContext: DbContext
    {
        public TodoDataContext(DbContextOptions<TodoDataContext> options)
            :base(options)
        { }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
