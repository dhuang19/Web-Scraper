using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebScraperAPI.Models;
using WebScraperAPI.Models.Graph;

namespace WebScraperAPI.Controllers
{
    /// <summary>
    /// Controller for API movie calls
    /// </summary>
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        public Graph Graph { get; set; }
        
        // GET movies?name=nameArg
        [HttpGet]
        public IEnumerable<string> GetFilter([FromQuery(Name = "name")] string nameArg)
        {
            return new string[] {nameArg};
        }
        
        // GET movies/name
        [HttpGet("{name}")]
        public string GetMovie(string name)
        {
            foreach (var node in Graph.GetNodes())
            {
                if (node is MovieNode mnode &&
                    (mnode.GetData() as MovieData)?.Title == name)
                {
                    return (mnode.GetData() as MovieData)?.Title;
                }
            }

            return "Not Found";
        }
    }
}