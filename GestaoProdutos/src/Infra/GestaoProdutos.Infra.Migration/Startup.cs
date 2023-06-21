using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentMigrator.Runner;

namespace GestaoProdutos.Infra.Migration;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services,
        string connectionString)
    {
        Console.WriteLine("Configurando recursos...");

        services.AddLogging(configure => configure.AddConsole());

        services.AddFluentMigratorCore()
            .ConfigureRunner(cfg => cfg
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Startup).Assembly).For.Migrations()
            )
            .AddLogging(cfg => cfg.AddFluentMigratorConsole());

        services.AddTransient<ConsoleApp>();
    }
}