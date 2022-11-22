using Microsoft.AspNetCore;
using AccountManagementService.Api;
using System.Diagnostics.CodeAnalysis;

[assembly: ExcludeFromCodeCoverage]

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

BuildWebHost(args).Run();

IWebHost BuildWebHost(string[] args) =>
    WebHost
        .CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseConfiguration(configuration)
        .Build();