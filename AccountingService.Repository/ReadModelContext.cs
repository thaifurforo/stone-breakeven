using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository;

public class ReadModelContext : DbContext
{
    public ReadModelContext(DbContextOptions<ReadModelContext> options)
        : base(options)
    {
    }
    
    public DbSet<Account> Accounts { get; set; }
    
    
}