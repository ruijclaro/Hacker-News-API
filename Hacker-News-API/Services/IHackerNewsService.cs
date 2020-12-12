using Hacker_News_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hacker_News_API.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetBestStories(int numberOfStories);
    }
}
