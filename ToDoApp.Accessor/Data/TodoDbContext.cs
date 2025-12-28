using Microsoft.EntityFrameworkCore;
using ToDoApp.Accessor.Models;

namespace ToDoApp.Accessor.Data;

public sealed class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoEntity> Todos => Set<TodoEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<TodoEntity>();

        e.ToTable("todos");
        e.HasKey(x => x.Id);

        e.Property(x => x.Title).IsRequired().HasMaxLength(200);
        e.Property(x => x.Description).HasMaxLength(1000);
        e.Property(x => x.IsCompleted).IsRequired();
        e.Property(x => x.CreatedAt).IsRequired();
    }
}
