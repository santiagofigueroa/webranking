using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.WebRanking.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _service;

        public SearchController(ISearchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchResult>>> GetSearchHistory()
        {    
            var histories = await _service.GetSearchHistoryAsync();
            return Ok(histories);
        }


        [HttpPost]
        public async Task<ActionResult> Search([FromBody] SearchResult search)
        {
            var result = await _service.GetSearchRankingsAsync(search);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IEnumerable<SearchEngine>> GetSearchEngines()
        {
            var searchEnginesLst = await _service.GetSearchEnginesAsync();
            return searchEnginesLst;
        }
    }
}
