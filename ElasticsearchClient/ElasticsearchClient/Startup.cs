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

        services.AddSingleton<IElasticClient>(client);

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}