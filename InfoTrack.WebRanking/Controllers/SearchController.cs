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

        public async Task<ActionResult> Index()
        {
            var history = await _service.GetSearchHistoryAsync();
            return View(history);
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchResult search)
        {
            search = await _service.GetSearchRankingsAsync(search);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IEnumerable<SearchEngine>> GetSearchEngines()
        {
            var searchEnginesLst = await _service.GetSearchEnginesAsync();
            return searchEnginesLst;
        }
    }
}
