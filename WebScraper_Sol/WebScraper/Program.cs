using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using WebScraper.Controllers;
using WebScraper.Models;
using WebScraper.Models.Graph;
using WebScraper.Utilities;

namespace WebScraper
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            
            MovieData startData = new MovieData("https://en.wikipedia.org/wiki/How_to_Train_Your_Dragon_(film)");

            ScraperController controller = new ScraperController(startData);
            
            Graph g = controller.BeginScraping();

            string path = "";
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
            {
                path = directoryInfo.FullName + "/JSONData";
            }

            IList<Node> nodes = g.GetNodes();
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path, true))
            {
                
                foreach (Node node in nodes)
                {
                    //Print out node
                    if (node is ActorNode actorNode)
                    {
                        ActorData data = actorNode.GetData() as ActorData;
                        if (data.Birthday != DateTime.MinValue)
                        {
                            Console.WriteLine("Name: " + data.Name);
                            Console.WriteLine("Total Gross: " + data.TotalGross);
                            Console.WriteLine("Birthday: " + data.Birthday);
                            Console.WriteLine("Starred in Movies: ");

                            if (data.Movies != null)
                            {
                                foreach (MovieData movieData in data.Movies)
                                {
                                    Console.WriteLine("    " + movieData.Title);
                                }
                            }
                            Console.WriteLine();
                            
                            if (data.Movies != null)
                            {
                                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                                file.WriteLine(jsonStr);
                            }
                        }
                    }
                    else if (node is MovieNode movieNode)
                    {
                        MovieData data = movieNode.GetData() as MovieData;
                        if (data.ReleaseDate == DateTime.MaxValue)
                        {
                            Console.WriteLine("Title: " + data.Title);
                            Console.WriteLine("Movie Gross: " + data.Gross);
                            Console.WriteLine("Release Date: " + data.ReleaseDate);
                            Console.WriteLine("Cast: ");

                            if (data.Actors != null)
                            {
                                foreach (ActorData actorData in data.Actors)
                                {
                                    Console.WriteLine("    " + actorData.Name);
                                }
                            }
                            Console.WriteLine();
                            
                            if (data.Actors != null)
                            {
                                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                                file.WriteLine(jsonStr);
                            }
                        }
                    }
                }
            }
        }
        
    }
}