using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Domain.Contracts;

public interface IDbContext
{
     DbSet<Account> Accounts { get; set; }

     int SaveChanges(bool acceptAllChangesOnSuccess);
}