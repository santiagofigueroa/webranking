using InfoTrack.WebRanking.Services;
using InfoTrack.WebRanking.Interfaces;
using InfoTrack.WebRanking.Models;
using Moq;

namespace InfoTrack.WebRanking.Tests
{
    [TestFixture]
    public class SearchServiceTests
    {
        private Mock<ISearchRepository> _mockSearchRepository;
        private SearchService _searchService;

        [SetUp]
        public void Setup()
        {
            _mockSearchRepository = new Mock<ISearchRepository>();
            _searchService = new SearchService(_mockSearchRepository.Object);
        }

        [Test]
        public async Task GetSearchHistoryAsync_ReturnsSearchHistory()
        {
            var mockData = new List<SearchHistory>
            {
                new SearchHistory { Keywords = "Test1" },
                new SearchHistory { Keywords = "Test2" }
            };
            _mockSearchRepository.Setup(r => r.GetSearchHistoryAsync()).ReturnsAsync(mockData);

            var result = await _searchService.GetSearchHistoryAsync();

            Assert.AreEqual(2, result.Count());
            _mockSearchRepository.Verify(r => r.GetSearchHistoryAsync(), Times.Once);
        }

        [Test]
        public async Task SaveSearchResultAsync_SavesSearchResult()
        {
            var search = new SearchResult { Keywords = "Test", Url = "http://google.com", ResultPositions = "1, 2" };
            _mockSearchRepository.Setup(r => r.SaveSearchResultAsync(It.IsAny<SearchHistory>())).Returns(Task.CompletedTask);

            await _searchService.SaveSearchResultAsync(search);

            _mockSearchRepository.Verify(r => r.SaveSearchResultAsync(It.IsAny<SearchHistory>()), Times.Once);
        }

        [Test]
        public async Task GetSearchEnginesAsync_ReturnsSearchEngines()
        {
            var mockData = new List<SearchEngine>
            {
                new SearchEngine { Title = "Google" },
                new SearchEngine { Title = "Bing" }
            };
            _mockSearchRepository.Setup(r => r.GetAllSearchEnginesAsync()).ReturnsAsync(mockData);

            var result = await _searchService.GetSearchEnginesAsync();

            Assert.AreEqual(2, result.Count());
            _mockSearchRepository.Verify(r => r.GetAllSearchEnginesAsync(), Times.Once);
        }

        [Test]
        public async Task GetSearchRankingsAsync_ReturnsUnchangedSearch_WhenSearchEngineIsNotFound()
        {
            var searchEngines = new List<SearchEngine>();
            var search = new SearchResult { Keywords = "test", SelectedSearchEngineId = 1 };

            _mockSearchRepository.Setup(r => r.GetAllSearchEnginesAsync()).ReturnsAsync(searchEngines);

            var result = await _searchService.GetSearchRankingsAsync(search);

            Assert.AreEqual(search, result);
        }

        [Test]
        public void ExtractSearchResultsFromResponse_ReturnsCorrectRanks()
        {
            var responseBody = "<div>some text</div><div>infotrack</div><div>other text</div>";
            var expression = "//div";

            var result = _searchService.ExtractSearchResultsFromResponse(responseBody, expression);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.First());
        }
    }
}
