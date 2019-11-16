using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebScraperAPI.Controllers;
using WebScraperAPI.Models;
using WebScraperAPI.Models.Graph;
using WebScraperAPI.Utilities;

namespace WebScraperAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            MovieData startData = new MovieData("https://en.wikipedia.org/wiki/Trainspotting_(film)");
            ScraperController controller = new ScraperController(startData, 5, 5);
            Graph g = controller.BeginScraping();

            MoviesController moviesController = new MoviesController {Graph = g};

            host.Run();
        }
    }
}