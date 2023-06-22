using GestaoProdutos.Application.Commands.DependencyInjection;
using GestaoProdutos.Application.Queries.DependencyInjection;
using GestaoProdutos.Infra.Database.DependencyInjection;
using GestaoProdutos.Infra.Repositories.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GestaoProdutos.Presenter.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSqlContext()
            .AddQueries()
            .AddRepositories()
            .AddCommandFactories();
        services.AddCors();
        services.AddMvc(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        services.AddControllers();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestão de Produtos", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestão de Produtos V1");
                c.RoutePrefix = string.Empty;
            });
        }
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseCors();
    }
}