using Hacker_News_API.Models;
using Hacker_News_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hacker_News_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyHackerNewsController : ControllerBase
    {

        private readonly IHackerNewsService _newsService;
        private readonly int _numberOfStories;

        public MyHackerNewsController(IHackerNewsService newsService, IConfiguration configuration)
        {
            _newsService = newsService;

            if (!int.TryParse(configuration["ServiceParameters:ReturnTopStories"], out _numberOfStories))
                throw new ApplicationException("ServiceParameters:ReturnTopStories not defined or not a valid number in configuration");
            
        }


        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> Get()
        {
            var stories = await _newsService.GetBestStories(_numberOfStories);
            return new ActionResult<IEnumerable<Story>>(stories);
        }

/*
    /// The following methods are left here, commented-out,
    /// just for illustrative purposes on how we would
    /// implement a more complete RESTful API 
    /// that would include typical CRUD operations.
 

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
*/
    }
}
