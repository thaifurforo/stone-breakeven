using System.Diagnostics;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository;

public class ReadModelInMemoryContext : DbContext
{
    public ReadModelInMemoryContext(DbContextOptions<ReadModelInMemoryContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    public DbSet<Account> Accounts { get; set; }

}