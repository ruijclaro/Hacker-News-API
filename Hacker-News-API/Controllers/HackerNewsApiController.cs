using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using LazyCache;

namespace Hacker_News_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsApiController : ControllerBase
    {
        private readonly IAppCache cache;

        public HackerNewsApiController(IAppCache cache)
        {
            this.cache = cache;
        }

        //Wrap the call you want to cache in a lambda and use the cache:

        //[HttpGet]
        //[Route("api/products")]
        //public IEnumerable<T> Get()
        //{
        //    // define a func to get the products but do not Execute() it
        //    Func<IEnumerable<T>> productGetter = () => dbContext.Products.ToList();

        //    // get the results from the cache based on a unique key, or 
        //    // execute the func and cache the results
        //    var productsWithCaching = cache.GetOrAdd("HomeController.Get", productGetter);

        //    return productsWithCaching;
        //}

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
    }
}
