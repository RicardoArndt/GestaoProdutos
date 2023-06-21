using GestaoProdutos.Infra.Migration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("**** GestaoProdutos - Execucao de Migrations ****");

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
Startup.ConfigureServices(services, configuration.GetConnectionString("GestaoProdutosDB"));
services.BuildServiceProvider()
    .GetService<ConsoleApp>()
    !.Run();