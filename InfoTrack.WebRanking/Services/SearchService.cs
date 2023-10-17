using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;

namespace InfoTrack.WebRanking.Services
{
    public class SearchService : ISearchService
    {
        public Task<string?> GetSearchHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveSearchResultAsync(SearchResult search)
        {
            throw new NotImplementedException();
        }
    }
}
