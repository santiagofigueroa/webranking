using InfoTrack.WebRanking.Models;

namespace InfoTrack.WebRanking.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync();
        Task SaveSearchResultAsync(SearchResult search);
        Task<SearchResult> GetSearchRankingsAsync(SearchResult search);
        List<int> ExtractSearchResultsFromResponse(string responseBody);
    }
}
