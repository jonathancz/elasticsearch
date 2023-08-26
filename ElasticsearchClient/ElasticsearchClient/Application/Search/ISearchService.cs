using ElasticsearchClient.Modules;
using ElasticsearchClient.Modules.Customer;
using ElasticsearchClient.Modules.Part;
using ElasticsearchClient.Modules.Subdivision;

namespace ElasticsearchClient.Application.Search;

public interface ISearchService
{
    Task<IEnumerable<Customer>> PerformCustomerSearch(string query, string indexName);
    Task<IEnumerable<Subdivision>> PerformSubdivisionSearch(string query, string indexName);
    Task<IEnumerable<ItemPart>>  PerformItemSearch(string query, string indexName);
}