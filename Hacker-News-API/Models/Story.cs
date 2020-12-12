using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//The API should return an array of the first 20 "best stories" as returned by the Hacker News API,
//sorted by their score in a descending order, in the form:
//[
//{
//"title": "A uBlock Origin update was rejected from the Chrome Web Store",
//"uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
//"postedBy": "ismaildonmez",
//"time": "2019-10-12T13:43:01+00:00",
//"score": 1716,
//"commentCount": 572
//},
//{ ... },
//{ ... },
//{ ... },
//...
//]
namespace Hacker_News_API.Models
{
    public class Story
    {
        public string Title {get;set;}
        public string Uri { get; set; }
        public string PostedBy { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public string CommentCount { get; set; }

    }
}
