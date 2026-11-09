using System;
using ExpenseApp.Application.Exceptions;
using ExpenseApp.Domain.Data.Abstractions;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Infrastructure.Data;

public class ExpenseDbContext : DbContext, IUnitOfWork
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            // await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }
}
