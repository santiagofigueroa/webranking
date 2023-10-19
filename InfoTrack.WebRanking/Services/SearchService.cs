using HtmlAgilityPack;
using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
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

            if (searchEngines == null) return search;
            //TODO:Find a  better way to chose between different search engines as is only for default at the moment
            var searchUrl = searchEngines.FirstOrDefault().SearchUrl.Replace("#SearchText#", HttpUtility.UrlEncode(search.Keywords)).Replace("#NumberResultsToSearchIn#", "100");

            using var client = new HttpClient();
            var response = HttpUtility.HtmlDecode(await client.GetStringAsync($"{searchEngines.FirstOrDefault().BaseUrl}/{searchUrl}"));
            var searchResults = ExtractSearchResultsFromResponse(response);

            var rankingList = (from link in searchResults
                               where link.Contains(search.Url, StringComparison.OrdinalIgnoreCase)
                               select searchResults.IndexOf(link) + 1)
                               .Distinct()
                               .ToList();

            search.ResultPositions = string.Join(", ", rankingList);

            await SaveSearchResultAsync(search);  

            return search;
        }

        public List<string> ExtractSearchResultsFromResponse(string responseBody)
        {
            var document = new HtmlDocument();
            document.LoadHtml(responseBody);

            var resultNodes = document.DocumentNode.SelectNodes("//div//h3/a[@href]");

            if (resultNodes == null)
                return new List<string>();

            return resultNodes
                .Select(node => HttpUtility.UrlDecode(node.Attributes["href"].Value))
                .ToList();
        }
    }
}
