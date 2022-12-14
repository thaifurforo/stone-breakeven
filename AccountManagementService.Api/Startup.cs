using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using AccountManagementService.Domain.Validators.Commands;
using AccountManagementService.Repository.Contexts;
using AccountManagementService.Repository.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementService.Api;

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
        
        // Contexts
        services.AddDbContext<ReadModelSqlContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("ReadModelSqlConnection")));
        services.AddDbContext<EventStoreSqlContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("EventStoreSqlConnection")));

        //Repositories
        services.AddScoped<IAccountRepository, AccountSqlRepository>();
        services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
        services.AddScoped<ITransactionRepository, TransactionSqlRepository>();
        
        //Validators
        services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
        services.AddScoped<IValidator<CreateTransactionCommand>, CreateTransactionCommandValidator>();

        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "AccountManagementService", Version = "v1" });
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