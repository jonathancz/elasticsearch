using ElasticsearchClient.Modules;
using Nest;

namespace ElasticsearchClient.Application.Search;

public class SearchService : ISearchService
{
    private readonly IElasticClient _elasticClient;

    public SearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public List<OmnisearchViewModel> PerformOmnisearch(string query)
    {
        // This can be moved to a Query or Repository layer.
        var searchResponse = _elasticClient.Search<Dictionary<string, object>>(s => s
            .Index("customer_index, subdivision_index, lot_index, buildingdepartment_index, deacomitem_index")
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f.Field("name").Field("description"))
                    .Query(query)
                )
            )
        );

        var results = new List<OmnisearchViewModel>();
        foreach (var document in searchResponse.Documents)
        {
            results.Add(new OmnisearchViewModel
            {
                EntityType = (document.ContainsKey("entityType") ? document["entityType"].ToString() : "Unknown")!,
                Entity = document
            });
        }

        return results;
    }
}