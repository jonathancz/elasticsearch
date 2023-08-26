using ElasticsearchClient.Modules.Customer;
using ElasticsearchClient.Modules.Part;
using ElasticsearchClient.Modules.Subdivision;
using Nest;
using Xunit;

namespace ElasticsearchClient.Persistence;

public class ElasticsearchIntegrationTests
{
    private readonly ElasticClient _client;
    private static readonly AutoResetEvent resetEvent = new AutoResetEvent(false); // Added for synchronization

    public ElasticsearchIntegrationTests()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
        _client = new ElasticClient(settings);
        Setup();
    }

    private void Setup()
    {
        // Consolidated index deletion and creation to a single method
        HandleIndices();

        // Generating data if indices do not exist
        GenerateDataIfNeeded("test_customer_index", () => GenerateCustomers(500));
        GenerateDataIfNeeded("test_subdivision_index", () => GenerateSubdivisions(500));
        GenerateDataIfNeeded("test_item_index", () => GenerateItems(500));
    }


    private void HandleIndices()
    {
        // Consolidated index deletion logic
        DeleteIndices("test_customer_index", "test_subdivision_index");
    }

    private void DeleteIndices(params string[] indexNames)
    {
        foreach (var indexName in indexNames)
        {
            _client.Indices.Delete(indexName);
        }
    }

    private void GenerateDataIfNeeded(string indexName, Action generateAction)
    {
        var indexExistsResponse = _client.Indices.Exists(indexName);
        if (!indexExistsResponse.Exists)
        {
            generateAction();
        }
    }

    [Fact]
    public void InitializeSeedElasticsearch()
    {
        resetEvent.WaitOne(); // Wait for data generation to complete
    }

    private void GenerateSubdivisions(int count)
    {
        var subdivisions = new SubdivisionFaker().Generate(count);
        BulkInsertSubdivision(subdivisions, "test_subdivision_index");
    }

    private void GenerateCustomers(int count)
    {
        var customers = new CustomerFaker().Generate(count);
        BulkInsertCustomer(customers, "test_customer_index");
    }

    private void GenerateItems(int count)
    {
        var items = new ItemPartFaker().Generate(count);
        BulkInsertCustomer(items, "test_item_index");
    }

    private void BulkInsertCustomer(IEnumerable<Customer> documents, string indexName)
    {
        var bulkAll = _client.BulkAll(documents, b => b
            .Index(indexName)
            .BackOffTime("30s")
            .BackOffRetries(2)
            .RefreshOnCompleted(true)
            .MaxDegreeOfParallelism(Environment.ProcessorCount)
            .Size(100)
        );

        bulkAll.Subscribe(new BulkAllObserver(
            onNext: (b) => Console.WriteLine($"Indexed page {b.Page}"),
            onError: (e) => Console.WriteLine($"An error occurred: {e.Message}"),
            onCompleted: () =>
            {
                Console.WriteLine("Bulk indexing complete");
                resetEvent.Set(); // Signal that data generation is complete
            }
        ));
    }

    private void BulkInsertCustomer(IEnumerable<ItemPart> documents, string indexName)
    {
        var bulkAll = _client.BulkAll(documents, b => b
            .Index(indexName)
            .BackOffTime("30s")
            .BackOffRetries(2)
            .RefreshOnCompleted(true)
            .MaxDegreeOfParallelism(Environment.ProcessorCount)
            .Size(100)
        );

        bulkAll.Subscribe(new BulkAllObserver(
            onNext: (b) => Console.WriteLine($"Indexed page {b.Page}"),
            onError: (e) => Console.WriteLine($"An error occurred: {e.Message}"),
            onCompleted: () =>
            {
                Console.WriteLine("Bulk indexing complete");
                resetEvent.Set(); // Signal that data generation is complete
            }
        ));
    }

    private void BulkInsertSubdivision(IEnumerable<Subdivision> documents, string indexName)
    {
        var bulkAll = _client.BulkAll(documents, b => b
            .Index(indexName)
            .BackOffTime("30s")
            .BackOffRetries(2)
            .RefreshOnCompleted(true)
            .MaxDegreeOfParallelism(Environment.ProcessorCount)
            .Size(100)
        );

        bulkAll.Subscribe(new BulkAllObserver(
            onNext: (b) => Console.WriteLine($"Indexed page {b.Page}"),
            onError: (e) => Console.WriteLine($"An error occurred: {e.Message}"),
            onCompleted: () =>
            {
                Console.WriteLine("Bulk indexing complete");
                resetEvent.Set(); // Signal that data generation is complete
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