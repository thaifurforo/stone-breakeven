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
        // services.AddDbContext<SqlReadModelContext>(options => 
        //     options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        services.AddDbContext<ReadModelContext>(opt =>
        {
            opt.UseInMemoryDatabase("Accounts");
        });
        services.AddEndpointsApiExplorer();
        services.AddScoped<IAccountRepository, AccountRepository>();
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