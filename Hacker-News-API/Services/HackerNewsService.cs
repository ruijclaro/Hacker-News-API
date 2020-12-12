using Hacker_News_API.Models;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

//The Hacker News API is documented here:
//https://github.com/HackerNews/API.


//The IDs for the "best stories" can be retrieved from this URI:
//https://hacker-news.firebaseio.com/v0/beststories.json. (this returns more than 20 story IDs)
// [25363366,25373591,25363981,25386756,25373462,25366484,25372464,25375979,25385833,25375711,25391202,25372356,25366253,
// 25369929,25367201,25366719,25372309,25364073,25371326,25391159,25391186,25383524,25365397,25375170,25370804,25383598,
// 25390941,25381242,25384433,25375575,25389340,25363777,25380999,25383976,25396745,25377695,25372336,25393198,25385014,
// 25377696,25368654,25386872,25372620,25372401,25371147,25384409,25372181,25371017,25373831,25395432,25385860,25385296,
// 25388343,25390967,25374140,25378155,25383119,25378551,25374383,25377620,25370716,25394965,25372812,25383623,25382529,
// 25383066,25372636,25382497,25373130,25389123,25384846,25373597,25384582,25381009,25373478,25389266,25374319,25369208,
// 25373105,25381325,25375148,25372987,25385137,25376849,25389219,25368379,25388353,25383485,25386482,25366301,25381397, ... 
// ]


//The details for an individual story ID can be retrieved from this URI:
//https://hacker-news.firebaseio.com/v0/item/21233041.json (in this case for the story with ID 21233041)
//{"by":"ismaildonmez","descendants":588,"id":21233041,"kids":[21233229,21233577,21235077,21233633,21233159,21233523,21233181,21233289,21233377,21233166,21233206,21233208,21233255,...],
// "score":1757,"time":1570887781,"title":"A uBlock Origin update was rejected from the Chrome Web Store","type":"story","url":"https://github.com/uBlockOrigin/uBlock-issues/issues/745"}

namespace Hacker_News_API.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IAppCache _cache;
        private readonly IConfiguration _configuration;
        private readonly string _getTopStoryIdsUri;
        private readonly string _getStoryUriFormat;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        /// <summary>
        /// Initial Constructor
        /// Read from AppSettings Cache Expiration Policy
        /// Read from AppSetting NewsHacker URIs
        /// </summary>
        /// <param name="cache">Cache</param>
        /// <param name="configuration">Configuration</param>
        public HackerNewsService(IAppCache cache, IConfiguration configuration)
        {
            _cache = cache;
            var cacheAbsoluteTimeExpiration = short.Parse(configuration["Cache:AbsoluteTimeExpiration"]);
            var cacheSlidingTimeExpiration = short.Parse(configuration["Cache:SlidingTimeExpiration"]);

            _cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, cacheAbsoluteTimeExpiration, 0),
                SlidingExpiration = new TimeSpan(0, cacheSlidingTimeExpiration, 0)
            };

            _configuration = configuration;
            _getTopStoryIdsUri = _configuration["HackerNewsAPI:BestStoryIdsUri"];
            _getStoryUriFormat = _configuration["HackerNewsAPI:GetStoryUriFormat"];
        }

        /// <summary>
        /// Get Top Stories Ids
        /// </summary>
        /// <returns>Ids</returns>
        private async Task<IEnumerable<string>> GetBestStoryIds()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_getTopStoryIdsUri);

                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("Error returnig Data from API");
                    throw new ApplicationException();
                }
                var ids = await response.Content.ReadAsAsync<IEnumerable<string>>();

                return ids;
            }
        }

        /// <summary>
        /// Transform UnixTimeStamp to DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns>DateTime</returns>
        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        /// <summary>
        /// Get HackerNews Story
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HackerNewsModel</returns>
        private async Task<HackerNewsModel> GetHackerNewsStory(string id)
        {
            using (var client = new HttpClient())
            {
                string requestUri = string.Format(_getStoryUriFormat, id);
                HttpResponseMessage response = await client.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    // Log the error
                    Log.Error("Error returnig Data from API");
                    throw new ApplicationException();
                }
                var hackerNewsStory = await response.Content.ReadAsAsync<HackerNewsModel>();
                return hackerNewsStory;
            }
        }

        /// <summary>
        /// Read List of Stories
        /// </summary>
        /// <param name="ids">Ids of each story</param>
        /// <returns>List of Stories</returns>
        private async Task<IEnumerable<Story>> GetStories(IEnumerable<string> ids)
        {
            var listOfStories = new List<Story>();
            foreach (var id in ids)
            {
                // Define a Lambda Expression to get the Data either from cache or Service if cache if not available for the Story with that Id
                var story = await _cache.GetOrAddAsync($"story: {id}", () => GetStory(id), _cacheEntryOptions);
                listOfStories.Add(story);
            }
            return listOfStories;
        }

        /// <summary>
        /// Map Properties from HackerNews Story to this service output
        /// </summary>
        /// <param name="id">Id of HackerNews Story</param>
        /// <returns>Story</returns>
        private async Task<Story> GetStory(string id)
        {
            var hackerNewsStory = await GetHackerNewsStory(id);
            var story = new Story
            {
                CommentCount = hackerNewsStory.Descendants,
                PostedBy = hackerNewsStory.By,
                Score = hackerNewsStory.Score,
                Time = UnixTimeStampToDateTime(hackerNewsStory.Time),
                Title = hackerNewsStory.Title,
                Uri = hackerNewsStory.Url
            };
            return story;
        }

        /// <summary>
        /// Read Best Stories, either from cache or from HackerNews Service
        /// </summary>
        /// <param name="numberOfStories"></param>
        /// <returns>Stories</returns>
        public async Task<IEnumerable<Story>> GetBestStories(int numberOfStories)
        {
            // Define a Lambda Expression to get the Data either from cache or Service if cache if not available all the Stories
            var ids = await _cache.GetOrAddAsync($"storyIds", GetBestStoryIds, _cacheEntryOptions);
            // Show only the x number of Best Stories based on configuration
            var firstIds = ids.Take(numberOfStories);
            // Order Stories Descending by Story Score
            var stories = (await GetStories(firstIds)).OrderByDescending(p => p.Score);

            return stories;
        }
    }
}
