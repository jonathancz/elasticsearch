using ElasticsearchClient.Application.Search;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    
    [HttpGet("searchSubdivisionByCustomerId")]
    public IActionResult SearchSubdivisionsByCustomerId([FromQuery] int customerId)
    {
        var subdivisions = _searchService.SearchSubdivisionsByCustomerId(customerId);
        if (subdivisions.Count > 0)
        {
            return Ok(subdivisions);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get(string query, string entityType)
    {
        if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(entityType))
        {
            return BadRequest("Query and entity type must be provided.");
        }

        string indexName = entityType.ToLower() switch
        {
            "customer" => "test_customer_index",
            "subdivision" => "test_subdivision_index",
            "item" => "test_item_index",
            _ => null
        };

        if (indexName == null)
        {
            return BadRequest("Invalid entity type.");
        }

        if (entityType.ToLower() == "customer")
        {
            var results = await _searchService.PerformCustomerSearch(query, indexName);
            return Ok(results);
        }
        else if (entityType.ToLower() == "subdivision")
        {
            var results = await _searchService.PerformSubdivisionSearch(query, indexName);
            return Ok(results);
        }
        else if (entityType.ToLower() == "item")
        {
            var results = await _searchService.PerformItemSearch(query, indexName);
            return Ok(results);
        }
        else
        {
            return BadRequest("Invalid entity type.");
        }
    }
}