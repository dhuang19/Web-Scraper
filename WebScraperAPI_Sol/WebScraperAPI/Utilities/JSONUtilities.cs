using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WebScraperAPI.Models;
using WebScraperAPI.Models.Graph;

namespace WebScraperAPI.Utilities
{
    /// <summary>
    /// Utilities for JSON Parsing
    /// </summary>
    public static class JSONUtilities
    {
        /// <summary>
        /// Deserializes JSON to graph
        /// </summary>
        /// <param name="path">Path JSON file is at</param>
        /// <returns>Graph with data</returns>
        public static Graph DeserializeJSON(string path)
        {
            Graph g = new Graph();
            string json = System.IO.File.ReadAllText(path);
            
/*            string s = "[ { \"name\": \"Test One\", \"address\": [ { \"street\": \"123\", \"city\": \"chicago\" },"
                       + " { \"street\": \"456\", \"city\": \"New York\" } ]}, "
                       + "{ \"name\": \"Test Two\", \"address\": [ { \"street\": \"567\", \"city\": \"Houston\" },"
                       + " { \"street\": \"987\", \"city\": \"Seattle\" } ] } ]";
            var array = JArray.Parse(s);
            for (int i = 0; i < array.Count; i++)
            {
                var obj = array[i];
                var name = obj.Value<string>("name");
                Console.WriteLine($"Name: {name}");
                var addressArray = (JArray)obj["address"];
                for (int j = 0; j < addressArray.Count; j++)
                {
                    var addressObj = addressArray[j];
                    var street = addressObj.Value<string>("street");
                    var city = addressObj.Value<string>("city");
                    Console.WriteLine($"Street: {street}  City: {city}");
                }
            }*/
            
            //Split into JSON objects
            JArray jsonObjs = JArray.Parse(json);
            JToken actorObjs = jsonObjs[0];    //Array of actor jsons
            JToken movieObjs = jsonObjs[1];    //Array of movie jsons

            for (int i = 0; i < actorObjs.Count(); i++)
            {
                var obj = actorObjs[i];
                var name = obj.Value<string>("name");
                var age = obj.Value<int>("age");
                var gross = obj.Values<long>("total_gross");
                
                var moviesArray = (JArray)obj["movies"];
                for (int j = 0; j < moviesArray.Count; j++)
                {
                    var movieObj = moviesArray[j];
                    var title = movieObj.Value<string>();
                }
            }

            return g;
        }
    }
}