using System;

namespace Hacker_News_API.Models
{
    /// <summary>
    /// Model for Story returned by this service
    /// </summary>
    public class Story
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string PostedBy { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }

    }
}
