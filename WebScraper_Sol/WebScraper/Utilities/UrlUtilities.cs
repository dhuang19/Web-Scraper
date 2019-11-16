using System;
using System.Net;
using HtmlAgilityPack;
using WebScraper.Models;

namespace WebScraper.Utilities
{
    /// <summary>
    /// Utilities for URL's
    /// </summary>
    public static class UrlUtilities
    {
        /// <summary>
        /// Loads a page to an HtmlDocument
        /// </summary>
        /// <param name="url">URL of page to load</param>
        /// <returns>HtmlDocument of page loaded</returns>
        public static HtmlDocument LoadPageToDoc(string url)
        {
            ThowExeptionIfInvalid(url);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            try
            {
                doc = web.Load(url);
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine(e);
                //TODO: Log this exception
                throw;
            }
            return doc;
        }
        
        /// <summary>
        /// Wrapper around LoadPageToDoc for input page parameters
        /// </summary>
        /// <param name="pageData">Input page</param>
        /// <returns>HtmoDocument of page loaded</returns>
        public static HtmlDocument LoadPageToDoc(Data pageData)
        {
            return LoadPageToDoc(pageData.GetUrl());
        }
        
        /// <summary>
        /// If URL is invalid, throw an exception
        /// </summary>
        /// <param name="url">Url to check</param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThowExeptionIfInvalid(string url)
        {
            if (!UrlUtilities.UrlIsValid(url))
            {
                //TODO: log this exception
                throw new ArgumentException("Url given to InputPage constructor is invalid.");
            }
        }

        /// <summary>
        /// If URL is relative, converts it to an absolute URL
        /// </summary>
        /// <param name="url">Relative URL</param>
        /// <<param name="baseUrl">Base URL</param>
        /// <returns>Absolute URL</returns>
        public static string ConvertToAbsoluteUrl(string url, string baseUrl)
        {
            //Check if url is relative
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Uri absoluteUri = new Uri(new Uri(baseUrl), url);
                return absoluteUri.ToString();
            }
            return url;
        }
        
        /// <summary>
        /// Checks if URL is valid (helper function)
        /// </summary>
        /// <param name="url">The URL to check</param>
        /// <returns>If URL is valid</returns>
        private static bool UrlIsValid(string url)
        {
            var web = new HtmlWeb();
            var document = web.Load(url);
            return web.StatusCode == HttpStatusCode.OK;
        }
    }
}