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

        Setup();
    }

    private void Setup()
    {
        _client.Indices.Delete("test_customer_index");
        _client.Indices.Delete("test_subdivision_index");
        
        var customerIndexExistsResponse = _client.Indices.Exists("test_customer_index");
        if (!customerIndexExistsResponse.Exists)
        {
            GenerateCustomers(50);
        }

        var subdivisionIndexExistsResponse = _client.Indices.Exists("test_subdivision_index");
        if (!subdivisionIndexExistsResponse.Exists)
        {
            GenerateSubdivisions(50);
        }
    }

    [Fact]
    public void InitializeSeedElasticsearch()
    {
        // Nothing else here.
    }

    private void GenerateSubdivisions(int count)
    {
        var subdivisionFaker = new SubdivisionFaker();
        var subdivisions = subdivisionFaker.Generate(count);

        var bulkAllSubdivision = _client.BulkAll(subdivisions, b => b
            .Index("test_subdivision_index")
            .BackOffTime("30s")
            .BackOffRetries(2)
            .RefreshOnCompleted(true)
            .MaxDegreeOfParallelism(Environment.ProcessorCount)
            .Size(100)
        );

        bulkAllSubdivision.Subscribe(new BulkAllObserver(
            onNext: (b) =>
            {
                // Log or print the processed page
                Console.WriteLine($"Indexed page {b.Page}");
            },
            onError: (e) =>
            {
                // Log or print the error
                Console.WriteLine($"An error occurred: {e.Message}");
            },
            onCompleted: () =>
            {
                // Log or print completion
                Console.WriteLine("Bulk indexing complete");
            }
        ));
    }

    private void GenerateCustomers(int count)
    {
        var customerFaker = new CustomerFaker();


        // Generate 500 fake customers and 500 fake subdivisions
        var customers = customerFaker.Generate(count);


        // Index these into Elasticsearch
        var bulkAllCustomer = _client.BulkAll(customers, b => b
            .Index("test_customer_index")
            .BackOffTime("30s")
            .BackOffRetries(2)
            .RefreshOnCompleted(true)
            .MaxDegreeOfParallelism(Environment.ProcessorCount)
            .Size(100)
        );


        bulkAllCustomer.Subscribe(new BulkAllObserver(
            onNext: (b) =>
            {
                // Log or print the processed page
                Console.WriteLine($"Indexed page {b.Page}");
            },
            onError: (e) =>
            {
                // Log or print the error
                Console.WriteLine($"An error occurred: {e.Message}");
            },
            onCompleted: () =>
            {
                // Log or print completion
                Console.WriteLine("Bulk indexing complete");
            }
        ));
    }

    [Fact(Skip = "Integration Test")]
    public void Should_Onboard_Data_Into_Indices()
    {
        _client.Indices.Refresh("test_customer_index");
        _client.Indices.Refresh("test_subdivision_index");


        // 4. Refresh the indices to make sure the data is searchable
        _client.Indices.Refresh("test_customer_index");
        _client.Indices.Refresh("test_subdivision_index");

        // 5. Search for the test data
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