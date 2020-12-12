namespace Hacker_News_API.Models
{
    /// <summary>
    /// Model for HackerNews API
    /// </summary>
    public class HackerNewsModel
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public string Id { get; set; }
        public string[] Kids { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

    }
}
