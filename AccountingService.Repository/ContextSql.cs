using AccountingService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Repository;

public class ContextSql : DbContext
{
    public ContextSql(DbContextOptions<ContextSql> options)
        : base(options)
    {
        this.Database.EnsureCreated();
        // O método Database.EnsureCreated() garante que o banco de dados para o contexto exista.
        // Se ele existir, nenhuma ação será realizada.
        // Se ele não existir, o banco de dados e to do o seu esquema serão criados.
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }
    
    public DbSet<Account> Accounts { get; set; }
}