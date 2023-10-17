using InfoTrack.WebRanking.Models;

namespace InfoTrack.WebRanking.Interfaces
{
    public interface ISearchRepository
    {
        Task SaveSearchResultAsync(SearchHistory result);
        Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync();
    }
}
