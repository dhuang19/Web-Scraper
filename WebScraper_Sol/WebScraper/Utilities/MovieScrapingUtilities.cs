using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlAgilityPack;
using WebScraper.Controllers;
using WebScraper.Models;

namespace WebScraper.Utilities
{
    /// <summary>
    /// Utilities for scraping Movie Wikipedia pages
    /// </summary>
    public static class MovieScrapingUtilities
    {
        /// <summary>
        /// Populates movie data with document
        /// </summary>
        /// <param name="movieData">MovieData to populate</param>
        /// <param name="doc">HtmlDocument to parse</param>
        /// <param name="controller">Scraper Controller</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static MovieData PopulateMovieData(MovieData movieData, HtmlDocument doc, ScraperController controller)
        {
            //Error handling
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "HtmlDocument can't be null.");
            if (movieData == null)
                throw new ArgumentNullException(nameof(movieData), "MovieData can't be null.'");
            
            //Retrieve data
            try
            {
                movieData.Title = Scrape_Movie_Title(doc);
                movieData.Gross = Scrape_Movie_Gross(doc);
                movieData.ReleaseDate = Scrape_Movie_Year(doc);
                movieData.Actors = Scrape_Movie_Actors(doc, controller);
            }
            catch (Exception e)
            {
                return null;
            }

            return movieData;
        }
        
        /// <summary>
        /// Scrapes HtmlDocument for movie title
        /// </summary>
        /// <param name="doc">HtmlDocument to parse</param>
        /// <returns>Movie Title</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Scrape_Movie_Title(HtmlDocument doc)
        {
            string title;
            try
            {
                title = doc.DocumentNode.SelectSingleNode(WikiXPaths.MovieScrapeTitle).InnerText;
            }
            catch (ArgumentNullException e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }
            
            if (title == null)
                throw new InvalidOperationException("SelectSingleNode in HtmlAgilityPack failed to extract movie title.");
            
            return title;
        }

        /// <summary>
        /// Scrapes HtmlDocument for movie gross
        /// </summary>
        /// <param name="doc">HtmlDocument to parse</param>
        /// <returns>Movie box office gross</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static long Scrape_Movie_Gross(HtmlDocument doc)
        {
            long gross;
            HtmlNode elem = doc.DocumentNode.SelectSingleNode(WikiXPaths.MovieScrapeGross).LastChild;
            string grossText = elem.LastChild.GetDirectInnerText();

            if (grossText[0] != '$')
            {
                //Found incorrect element
                return 0;
            }
            
            gross = StringToLong(grossText);

            if (gross != 0)
            {
                return gross;
            }
            else
            {
                throw new InvalidOperationException("Could not properly convert gross string representation to long.");
            }
        }

        /// <summary>
        /// Scrapes HtmlDocument for movie's release date
        /// </summary>
        /// <param name="doc">HtmlDocument to scrape</param>
        /// <returns>Release date</returns>
        public static DateTime Scrape_Movie_Year(HtmlDocument doc)
        {
            //TODO: fix this function
            //Retrieve tr element containing release date
            /*HtmlNode trElem;
            try
            {
                trElem = doc.DocumentNode.SelectSingleNode("//table[@class='infobox vevent']//tbody//tr//th//div[.='Release date']");
            }
            catch (ArgumentNullException e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }

            trElem = trElem.ParentNode.ParentNode;
            HtmlNode liElem = trElem.ChildNodes.FindFirst("//li");*/
            
            return DateTime.MaxValue;
        }

        /// <summary>
        /// Scrapes HtmlDocument for actors in movie
        /// </summary>
        /// <param name="doc">HtmlDocument to scrape</param>
        /// <param name="controller">Scraper Controller</param>
        /// <returns></returns>
        public static ICollection<ActorData> Scrape_Movie_Actors(HtmlDocument doc, ScraperController controller)
        {
            //TODO: Add exceptions for when unable to scrape
            ICollection<ActorData> dataCollection = new Collection<ActorData>();

            HtmlNode castNode;
            //Find 'Cast' header
            try
            {
                castNode = doc.DocumentNode.SelectSingleNode(WikiXPaths.MovieScrapeCastHeader);
            }
            catch (ArgumentNullException e)
            {
                //Cast node does not exist
                Console.WriteLine(e);
                return dataCollection;
            }

            if (castNode == null)
            {
                //Cannot find Cast
                return dataCollection;
            }

            castNode = castNode.ParentNode;

            //Loop to find <ul> element
            HtmlNode ulElem = castNode.NextSibling;
            while (ulElem != null)
            {
                if (ulElem.Name == "ul")
                {
                    break;
                }
                ulElem = ulElem.NextSibling;
            }
            
            //Loop through <ul> to find all <a> elements
            foreach (HtmlNode entry in ulElem.SelectNodes(".//li"))
            {
                HtmlNode link = entry.SelectSingleNode(".//a");
                
                ActorData actorData = new ActorData(UrlUtilities
                        .ConvertToAbsoluteUrl(link.Attributes["href"].Value, WikiXPaths.WikiBaseUrl),
                    link.InnerText);
                
                //Add to collection
                dataCollection.Add(actorData);
                
                //Add to controller
                controller.AddTask(actorData);
            }
            
            return dataCollection;
        }

        /// <summary>
        /// Helper function to transform string gross to int
        /// </summary>
        /// <param name="gross"></param>
        /// <returns></returns>
        private static long StringToLong(string gross)
        {
            var punctuation = gross.Where(Char.IsPunctuation).Distinct().ToArray();
            List<string> words = gross.Split().Select(x => x.Trim(punctuation)).ToList();
            
            //Remove $ sign
            words[0] = words[0].TrimStart('$');

            float num = 0;
            long multiplier = 0;
            for (int i = 0; i < words.Count; i++)
            {
                if (i == 0)
                {
                    num = float.Parse(words[i]);
                }
                else
                {
                    if (words[i] == "million")
                    {
                        multiplier = 1000000;
                    } 
                    else if (words[i] == "billion")
                    {
                        multiplier = 1000000000;
                    } else if (words[i] == "trillion")
                    {
                        multiplier = 1000000000000;
                    }
                }
            }

            return (long) (num * multiplier);
        }
    }
}