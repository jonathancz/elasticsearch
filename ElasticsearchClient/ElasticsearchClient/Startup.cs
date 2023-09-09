using ElasticsearchClient.Application.Search;
using Microsoft.OpenApi.Models;
using Nest;

namespace ElasticsearchClient;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Elasticsearch configuration
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("my_index");

        var client = new ElasticClient(settings);

        // Registering Services
        services.AddSingleton<IElasticClient>(client);
        services.AddScoped<ISearchService, SearchService>();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddControllers();
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Replace with your React app's URL
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();
        app.UseRouting();
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        app.UseCors();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}