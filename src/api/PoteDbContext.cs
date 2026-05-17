using Microsoft.EntityFrameworkCore;
using SimpleTodo.Api;
using src.DbModels;

public class PoteDbContext : DbContext
{
    public PoteDbContext(DbContextOptions options) : base(options) { }
    public DbSet<TodoItem> Items => Set<TodoItem>();
    public DbSet<TodoList> Lists => Set<TodoList>();
    public DbSet<InterestingLink> InterestingLinks => Set<InterestingLink>();
}