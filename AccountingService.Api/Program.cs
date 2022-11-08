using AccountingService.Domain.Contracts;
using AccountingService.Repository;
using AccountingService.Repository.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ReadModelContext>(opt =>
{
    opt.UseInMemoryDatabase("Accounts");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "AccountingService", Version = "v1" });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();