using System;

namespace Hacker_News_API.Models
{
    public class HackerNewsModel
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public int Id { get; set; }
        public int[] Kids { get; set; }
        public int Score { get; set; }
        public long Time { get; set; }
        public string Title { get; set; }
    }
}
