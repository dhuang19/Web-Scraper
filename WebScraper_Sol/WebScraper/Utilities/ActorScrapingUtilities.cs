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
    /// Utilities for scraping Actor Wikipedia pages
    /// </summary>
    public static class ActorScrapingUtilities
    {
        /// <summary>
        /// Populates actor data with the document
        /// </summary>
        /// <param name="actorData">Actor data to populate</param>
        /// <param name="doc">HtmlDocument to retrieve data from</param>
        /// <param name="controller">Scraper controller</param>
        public static ActorData PopulateActorData(ActorData actorData, HtmlDocument doc, ScraperController controller)
        {
            //Error handling
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "HtmlDocument can't be null.");
            if (actorData == null)
                throw new ArgumentNullException(nameof(actorData), "ActorData can't be null.'");
            
            //Retrieve data
            try
            {
                actorData.Name = Scrape_Actor_Name(doc);
                actorData.Birthday = Scrape_Actor_Birthday(doc);
                actorData.Movies = Scrape_Actor_Movies(doc, controller);
            }
            catch (Exception e)
            {
                return null;
            }

            return actorData;
        }

        /// <summary>
        /// Scrapes HtmlDocument for actor name
        /// </summary>
        /// <param name="doc">HtmlDocument to scrape</param>
        /// <returns>Actor name</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Scrape_Actor_Name(HtmlDocument doc)
        {
            string name;
            try
            {
                name = doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeName).InnerText;
            }
            catch (ArgumentNullException e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }
            
            if (name == null)
                throw new InvalidOperationException("SelectSingleNode in HtmlAgilityPack failed to extract actor name.");
            
            return name;
        }

        /// <summary>
        /// Scrapes HtmlDocument for actor birthday
        /// </summary>
        /// <param name="doc">HtmlDocument to scrape</param>
        /// <returns>Actor birthday</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static DateTime Scrape_Actor_Birthday(HtmlDocument doc)
        {
            string bday;
            DateTime dt;
            
            //Extract birthday as string
            try
            {
                bday = doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeBirthday).InnerText;
            }
            catch (ArgumentNullException e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }
            
            if (bday == null) 
                throw new InvalidOperationException("SelectSingleNode in HtmlAgilityPack failed to extract actor birthday.");

            //Convert string to DateTime
            try
            {
                dt = DateTime.Parse(bday);
            }
            catch (Exception e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Scrapes HtmlDocument for movies this actor has been in
        /// </summary>
        /// <param name="doc">HtmlDocument to scrape</param>
        /// <param name="controller">Scraper controller</param>
        /// <returns>Collection of Movie objects with just title initialized </returns>
        public static ICollection<MovieData> Scrape_Actor_Movies(HtmlDocument doc, ScraperController controller)
        {
            //TODO: Add exceptions for when unable to scrape
            //Check if redirects to main filmography article
            HtmlNode mainArticleNode;
            try
            {
                mainArticleNode = doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeFilmArticle);
            }
            catch (ArgumentNullException e)
            {
                //TODO: log exception
                Console.WriteLine(e);
                throw;
            }

            //Main filmography article exists, extract that URL and parse page
            if (mainArticleNode != null)
            {
                foreach (HtmlNode node in mainArticleNode.ChildNodes)
                {
                    if (node.Name == "a")
                    {
                        string url = UrlUtilities.ConvertToAbsoluteUrl(node.Attributes["href"].Value, 
                            WikiXPaths.WikiBaseUrl);
                        HtmlDocument mainArticleDoc = UrlUtilities.LoadPageToDoc(url);
                        return Scrape_Actor_Movies_MainArticle(mainArticleDoc, controller);
                    }
                }
                
                //Is there an exception for cases that should not happen?
                throw new Exception("Main article node exists without a link to it.");
            }
            
            //Filmography baked into current page
            HtmlNode bakedTableNode;
            try
            {
                bakedTableNode = doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeBakedMoviesTable);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                throw;
            }

            //If can't find, try alternative XPath
            if (bakedTableNode == null)
            {
                try
                {
                    bakedTableNode =
                        doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeBakedMoviesTableAlternative);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            if (bakedTableNode == null)
            {
                throw  new InvalidOperationException("Cannot find baked in film table.");
            }
            
            //Add tasks to controller
            ICollection<MovieData> dataCollection = RetrieveActorsInTable(bakedTableNode, WikiXPaths.ActorScrapeBakedMoviesTableEntries);
            //Add MovieData tasks to controller
            foreach (var data in dataCollection)
            {
                controller.AddTask(data);
            }

            return dataCollection;
        }

        /// <summary>
        /// Helper for scraping Actor's main filmography article
        /// </summary>
        /// <param name="doc">HtmlDocument of main filmography article</param>
        /// <param name="controller"></param>
        /// <returns>Collection of movies Actor was in</returns>
        private static ICollection<MovieData> Scrape_Actor_Movies_MainArticle(HtmlDocument doc,
            ScraperController controller)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "HtmlDocument can't be null.");
            
            //Retrieve Movie Table
            HtmlNode tableNode = doc.DocumentNode.SelectSingleNode(WikiXPaths.ActorScrapeFilmArticleTable);
            ICollection<MovieData> dataCollection = RetrieveActorsInTable(tableNode, WikiXPaths.ActorScrapeFilmArticleEntries);
            
            //Add MovieData tasks to controller
            foreach (var data in dataCollection)
            {
                controller.AddTask(data);
            }

            return dataCollection;
        }

        /// <summary>
        /// Helper function that retrieves all movies from a table node
        /// </summary>
        /// <param name="tableNode">Table node</param>
        /// <<param name="entriesXPath">XPath to use when finding entries</param>
        /// <returns>Movie entries in table</returns>
        private static ICollection<MovieData> RetrieveActorsInTable(HtmlNode tableNode, string entriesXPath)
        {
            ICollection<MovieData> dataCollection = new Collection<MovieData>();
            
            //TODO: Known bug: Cannot detect between short films, documentaries, and entries that have no hyperlinks (as a result, may include some TV)
            
            //Get number of movies
            HtmlNodeCollection tbodyChildren = tableNode.ChildNodes[1].ChildNodes;
            int moviesNum = 0;
            foreach (HtmlNode trElem in tbodyChildren)
            {
                if (trElem.Name == "tr")
                {
                    moviesNum++;
                }
            }

            int idx = 1;
            //Iterate through movies table
            foreach (HtmlNode entry in tableNode.SelectNodes(entriesXPath))
            {
                if (idx < moviesNum)
                {
                    MovieData movieData = new MovieData(UrlUtilities
                            .ConvertToAbsoluteUrl(entry.Attributes["href"].Value, WikiXPaths.WikiBaseUrl),
                        entry.InnerText);
                    
                    //Add new Movie obj to Collection
                    dataCollection.Add(movieData);
                }
                
                idx++;
            }

            return dataCollection;
        }
    }
}