using Hacker_News_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hacker_News_API.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetBestStories(int numberOfStories);
    }
}
