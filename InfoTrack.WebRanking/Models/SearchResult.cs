using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfoTrack.WebRanking.Models
{

    public class SearchResult
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
        public string ResultPositions { get; set; }
        public int SelectedSearchEngineId { get; set; }
        public List<SelectListItem> AvailableSearchEngines { get; set; } = new List<SelectListItem>();
    }

}
