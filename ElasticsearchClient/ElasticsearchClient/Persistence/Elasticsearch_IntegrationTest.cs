using ElasticsearchClient.Modules.Customer;
using ElasticsearchClient.Modules.Subdivision;
using Nest;
using Xunit;

namespace ElasticsearchClient.Persistence;

public class ElasticsearchIntegrationTests
{
    private readonly ElasticClient _client;

    public ElasticsearchIntegrationTests()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200")); // Replace with your Elasticsearch URI
        _client = new ElasticClient(settings);
    }

    [Fact]
    public void Should_Onboard_Data_Into_Indices()
    {
        // 1. Delete indices if they exist (useful for repeatable tests)
        _client.Indices.Delete("test_customer_index");
        _client.Indices.Delete("test_subdivision_index");

        // 2. Create new indices
        _client.Indices.Create("test_customer_index", c => c
            .Map(m => m
                .AutoMap<Customer>()));

        _client.Indices.Create("test_subdivision_index", c => c
            .Map(m => m
                .AutoMap<Subdivision>()));

        // 3. Index some test data


        // 4. Refresh the indices to make sure the data is searchable
        _client.Indices.Refresh("test_customer_index");
        _client.Indices.Refresh("test_subdivision_index");

        // 5. Search for the test data to verify it's been indexed
        var searchCustomerResponse = _client.Search<Customer>(s => s
            .Index("test_customer_index")
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query("John"))));

        var searchSubdivisionResponse = _client.Search<Subdivision>(s => s
            .Index("test_subdivision_index")
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query("Downtown"))));

        // 6. Assert that the data has been indexed
        Assert.NotEmpty(searchCustomerResponse.Documents);
        Assert.NotEmpty(searchSubdivisionResponse.Documents);
    }
}