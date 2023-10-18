namespace InfoTrack.WebRanking.Models
{
    public class SearchEngine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BaseUrl { get; set; }
        public string SearchUrl { get; set; }
        public string ResultExtractionExpression { get; set; }
    }
}