﻿using ElasticsearchClient.Modules;
using ElasticsearchClient.Modules.Customer;
using ElasticsearchClient.Modules.Part;
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
        // Create a new search descriptor
        SearchDescriptor<Customer> searchDescriptor = new SearchDescriptor<Customer>()
            .Size(1000);

        // If indexName is provided, set the index
        if (!string.IsNullOrEmpty(indexName))
        {
            searchDescriptor = searchDescriptor.Index(indexName);
        }

        // Determine the type of query (MatchAll or Match based on 'query')
        if (string.IsNullOrEmpty(query))
        {
            // MatchAll query to get all documents
            searchDescriptor = searchDescriptor.Query(q => q.MatchAll());
        }
        else
        {
            // Match query for the specified field
            searchDescriptor = searchDescriptor.Query(q => q
                .Match(m => m
                    .Field(f => f.Name)
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                )
            );
        }

        // Perform the search
        var searchResponse = await _elasticClient.SearchAsync<Customer>(searchDescriptor);

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

    public async Task<IEnumerable<ItemPart>> PerformItemSearch(string query, string indexName)
    {
        var searchResponse = await _elasticClient.SearchAsync<ItemPart>(s => s
            .Index(indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Partcode)
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                )
            )
        );
        return searchResponse.Documents;
    }

    public List<Subdivision> SearchSubdivisionsByCustomerId(int customerId)
    {
        var searchResponse = _elasticClient.Search<Subdivision>(s => s
            .Index("test_subdivision_index")
            .Query(q => q
                .Term(t => t
                    .Field(f => f.CustomerId)
                    .Value(customerId)
                )
            )
        );

        if (searchResponse.IsValid)
        {
            return searchResponse.Documents.ToList();
        }
        else
        {
            return new List<Subdivision>();
        }
    }
}