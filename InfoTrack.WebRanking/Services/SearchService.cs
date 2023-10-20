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

            if (selectedSearchEngine == null) return search;

            var searchUrlBuilder = new StringBuilder(selectedSearchEngine.BaseUrl);
            searchUrlBuilder.Append(selectedSearchEngine.SearchUrl);
            searchUrlBuilder.Replace("#SearchText#", HttpUtility.UrlEncode(search.Keywords));
            var searchUrl = searchUrlBuilder.ToString();

            //Accepts the cookie window option 
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler);

            // Pretend to be a browser to avoid bot blocking
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = HttpUtility.HtmlDecode(await client.GetStringAsync($"{searchUrl}"));

            // Extract rankings using the expression from the search engine model
            var rankingList = ExtractSearchResultsFromResponse(response, selectedSearchEngine.ResultExtractionExpression);

            //Place the results  in the corresponding list 
            search.ResultPositions = string.Join(", ", rankingList);
           
            //Save search results to database 
            await SaveSearchResultAsync(search);

            return search;
        }


        // Extraction method:
        public List<int> ExtractSearchResultsFromResponse(string responseBody, string expression)
        {
            var document = new HtmlDocument();
            document.LoadHtml(responseBody);

            // Use the provided expression for the search results extraction
            var resultNodes = document.DocumentNode.SelectNodes(expression);

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
