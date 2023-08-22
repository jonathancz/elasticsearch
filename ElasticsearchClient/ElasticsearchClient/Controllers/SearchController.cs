using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchClient.Application.Search;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("omnisearch")]
    public IActionResult Omnisearch(string query)
    {
        var results = _searchService.PerformOmnisearch(query);
        return Ok(results);
    }
}