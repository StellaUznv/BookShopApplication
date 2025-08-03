using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] string filter = "All", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Message = "Query is required." });

            var result = await _searchService.SearchAsync(query, filter, page, pageSize);
            return Ok(result);
        }
    }

}
