﻿using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Compatibility;

namespace TodoApp.Data.SupportedVersions
{
    [SchemaBinder(101)]
    public class V101ModelBinder : IVersionedModelBinder
    {
        public void BindModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().Ignore(p => p.Assignee);
            modelBuilder.Entity<TodoItem>().Ignore(p => p.StoryId);
        }
    }
}
