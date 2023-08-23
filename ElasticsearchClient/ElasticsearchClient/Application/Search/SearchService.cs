using ElasticsearchClient.Modules;
using ElasticsearchClient.Modules.Customer;
using ElasticsearchClient.Modules.Subdivision;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticsearchClient.Application.Search;

public class SearchService : ISearchService
{
    private readonly IElasticClient _elasticClient;

    public SearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IEnumerable<Customer>> PerformCustomerSearch(string query, string indexName)
    {
        var searchResponse = await _elasticClient.SearchAsync<Customer>(s => s
            .Index(indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                )
            )
        );
        return searchResponse.Documents;
    }

    public async Task<IEnumerable<Subdivision>> PerformSubdivisionSearch(string query, string indexName)
    {
        var searchResponse = await _elasticClient.SearchAsync<Subdivision>(s => s
            .Index(indexName)
            .Query(q => q
                .QueryString(d => d
                    .Query(query))));
        return searchResponse.Documents;
    }
}