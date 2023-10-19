using HtmlAgilityPack;
using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace InfoTrack.WebRanking.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync()
        {
            return await _searchRepository.GetSearchHistoryAsync();
        }

        public async Task SaveSearchResultAsync(SearchResult search)
        {
            var searchHistory = new SearchHistory
            {
                Keywords = search.Keywords,
                Url = search.Url,
                ResultPositions = search.ResultPositions,
                SearchDate = DateTime.Now 
            };

            await _searchRepository.SaveSearchResultAsync(searchHistory);
        }

        public async Task<SearchResult> GetSearchRankingsAsync(SearchResult search)
        {
            var searchEngines = await _searchRepository.GetAllSearchEnginesAsync();
            var selectedSearchEngine = searchEngines.FirstOrDefault(x => x.Id.Equals(search.SelectedSearchEngineName));

            if (selectedSearchEngine == null)
                return search;

            // Use StringBuilder for constructing the search URL
            var searchUrlBuilder = new StringBuilder();
            searchUrlBuilder.Append(selectedSearchEngine.BaseUrl);
            searchUrlBuilder.Append("/search?q=");
            searchUrlBuilder.Append(HttpUtility.UrlEncode(search.Keywords));
            searchUrlBuilder.Append("&num=100");
            var searchUrl = searchUrlBuilder.ToString();

            //Accepts the cookie window option 
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler);

            // Pretend to be a browser
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await client.GetStringAsync(searchUrl);
            var decodedResponse = HttpUtility.HtmlDecode(response);
            var rankings = ExtractSearchResultsFromResponse(decodedResponse);

            // This will convert the integer list into a comma-separated string.
            search.ResultPositions = string.Join(", ", rankings.Distinct());

            // Save or do further operations on the search result here.

            return search;
        }



        public List<int> ExtractSearchResultsFromResponse(string responseBody)
        {
            var document = new HtmlDocument();
            document.LoadHtml(responseBody);

            // Assuming Google search results are contained within div elements (you might want to adjust this selector)
            var resultNodes = document.DocumentNode.SelectNodes("//div[@class='MjjYud']");

            if (resultNodes == null)
                return new List<int>();

            var ranks = new List<int>();

            for (int i = 0; i < resultNodes.Count && i < 100; i++) // Limiting to the first 100 results
            {
                var resultNode = resultNodes[i];

                // Check if the content of the node contains "infotrack"
                if (resultNode.InnerText.Contains("infotrack", StringComparison.OrdinalIgnoreCase))
                {
                    // Add the rank (index + 1 since index starts at 0) to the list
                    ranks.Add(i + 1);
                }
            }

            return ranks;
        }


    }
}
