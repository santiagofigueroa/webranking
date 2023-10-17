using InfoTrack.WebRanking.Models;

namespace InfoTrack.WebRanking.Interfaces
{
    public interface ISearchService
    {
        Task<string?> GetSearchHistoryAsync();
        Task SaveSearchResultAsync(SearchResult search);
    }
}
