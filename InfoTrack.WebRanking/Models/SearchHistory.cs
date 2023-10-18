namespace InfoTrack.WebRanking.Models
{
    public class SearchHistory
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
        public string ResultPositions { get; set; }
        public DateTime SearchDate { get; set; }
    }
}