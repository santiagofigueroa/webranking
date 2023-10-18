using Dapper;
using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace InfoTrack.WebRanking.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SearchRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<SearchEngine>> GetAllSearchEnginesAsync()
        {
            const string sql = "SELECT * FROM SearchEngines";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<SearchEngine>(sql);
            }
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
