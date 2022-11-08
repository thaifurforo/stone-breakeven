using System.Diagnostics;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository;

public class ContextInMemory : DbContext
{
    public ContextInMemory(DbContextOptions<ContextInMemory> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    public DbSet<Account> Accounts { get; set; }

}