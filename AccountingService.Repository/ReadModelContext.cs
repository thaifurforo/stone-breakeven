using System.Diagnostics;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository;

public class ReadModelContext : DbContext
{
    public ReadModelContext(DbContextOptions<ReadModelContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    public DbSet<Account> Accounts { get; set; }

}