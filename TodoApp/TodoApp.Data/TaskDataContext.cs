using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data
{
    public class TaskDataContext: DbContext
    {
        public TaskDataContext(DbContextOptions<TaskDataContext> options)
            :base(options)
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}
