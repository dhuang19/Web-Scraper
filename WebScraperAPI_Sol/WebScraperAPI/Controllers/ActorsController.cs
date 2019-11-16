using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WebScraperAPI.Controllers
{
    [Route("[controller]")]
    public class ActorsController : Controller
    {
        // GET actors?name=nameArg
        [HttpGet]
        public IEnumerable<string> GetFilter([FromQuery(Name = "name")] string nameArg)
        {
            return new string[] {nameArg};
        }

        // GET actors/name
        [HttpGet("{name}")]
        public string GetActor(string name)
        {
            return name;
        }
        
        // PUT actors/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // POST actors
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // DELETE actors/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}