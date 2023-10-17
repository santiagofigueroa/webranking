using InfoTrack.WebRanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.WebRanking.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchResult search)
        {
            search.ResultPositions = ScrapeGoogle(search.Keywords, search.Url);
            return View("Index", search);
        }

        private string ScrapeGoogle(string keywords, string url)
        {
            // Your scraping logic here
            // You would use `HttpClient` or `WebClient` to fetch the HTML and then parse the HTML to get the rankings.
            // This would involve a bit of regex or string manipulation.
            // Return positions as string e.g., "1, 10, 33"
            // If the URL is not found within the first 100 results, return "0".

            return "";
        }
    }

}
