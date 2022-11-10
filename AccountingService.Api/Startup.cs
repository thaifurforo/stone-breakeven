using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Validators.Commands;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using FluentValidation;
using MediatR;
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
        services.AddMvc();
        services.AddControllers();
        services.AddMediatR(typeof(CreateAccountCommand));
        services.AddDbContext<ContextSql>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        services.AddDbContext<ContextInMemory>(opt =>
        {
            opt.UseInMemoryDatabase("Accounts");
        });
        // services.AddScoped<IAccountRepository, AccountRepositoryInMemory>();
        services.AddScoped<IAccountRepository, AccountRepositorySql>();
        services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
        
        services.AddEndpointsApiExplorer();
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
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });       
    }
}