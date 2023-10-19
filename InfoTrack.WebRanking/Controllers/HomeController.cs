using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace InfoTrack.WebRanking.Controllers
{
    public class HomeController : Controller
    {
        public readonly ISearchRepository _searchRepository; 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ISearchRepository searchRepository,
                              ILogger<HomeController> logger)
        {
            _searchRepository = searchRepository;  
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SearchResult();

            var searchEngines = await _searchRepository.GetAllSearchEnginesAsync();

            model.AvailableSearchEngines = searchEngines.Select(se => new SelectListItem
            {
                Value = se.Id.ToString(),
                Text = se.Title

            }).ToList();

            return View(model);
        }

    }
}