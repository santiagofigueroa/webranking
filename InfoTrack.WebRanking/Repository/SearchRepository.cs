using Dapper;
using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using System.Data.SqlClient;

namespace InfoTrack.WebRanking.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private IConfiguration _configuration;
        private readonly ISearchRepository _databaseService;
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public SearchRepository(IConfiguration configuration ,ISearchRepository databaseService)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _databaseService = databaseService;
        }

        public async Task SaveSearchResultAsync(SearchHistory result)
        {
            const string sql = "INSERT INTO SearchHistory (Keywords, Url, ResultPositions, SearchDate) VALUES (@Keywords, @Url, @ResultPositions, @SearchDate)";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, result);
            }
        }

        public async Task<IEnumerable<SearchHistory>> GetSearchHistoryAsync()
        {
            const string sql = "SELECT * FROM SearchHistory ORDER BY SearchDate DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<SearchHistory>(sql);
            }
        }
    }

}
