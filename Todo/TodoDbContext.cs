﻿using Microsoft.EntityFrameworkCore;

namespace Todo
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<Todo.Models.Todo> Todos => Set<Todo.Models.Todo>();
    }
}