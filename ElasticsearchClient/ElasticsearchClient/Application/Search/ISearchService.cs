using ElasticsearchClient.Modules;

namespace ElasticsearchClient.Application.Search;

public interface ISearchService
{
    List<OmnisearchViewModel> PerformOmnisearch(string query);
}