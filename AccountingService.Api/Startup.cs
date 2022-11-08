using AccountingService.Domain.Contracts;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Api;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<ContextSql>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        services.AddDbContext<ContextInMemory>(opt =>
        {
            opt.UseInMemoryDatabase("Accounts");
        });
        services.AddEndpointsApiExplorer();
        // services.AddScoped<IAccountRepository, AccountRepositoryInMemory>();
        services.AddScoped<IAccountRepository, AccountRepositorySql>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "AccountingService", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseSwagger();
        
    }
}