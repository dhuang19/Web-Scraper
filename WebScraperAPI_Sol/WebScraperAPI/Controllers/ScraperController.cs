using System;
using System.Collections.Generic;
using WebScraperAPI.Interfaces;
using WebScraperAPI.Models;
using WebScraperAPI.Models.Graph;
using WebScraperAPI.Utilities;

namespace WebScraperAPI.Controllers
{
    /// <summary>
    /// Controller for the flow of scraping
    /// </summary>
    public class ScraperController : ISraperController
    {
        /// <summary>
        /// Page to start parsing on
        /// </summary>
        private readonly Data _startPageData;

        /// <summary>
        /// Maximum actor pages to parse
        /// </summary>
        private int _maxActorPages = 250;

        /// <summary>
        /// Maximum movie pages to parse
        /// </summary>
        private int _maxMovePages = 125;

        /// <summary>
        /// Queue of movie pages to scrape
        /// </summary>
        private readonly Queue<MovieData> _movieInputs;

        /// <summary>
        /// Queue of actor pages to scrape
        /// </summary>
        private readonly Queue<ActorData> _actorInputs;

        public ScraperController(Data startPageData)
        {
            _startPageData = startPageData;
            _movieInputs = new Queue<MovieData>();
            _actorInputs = new Queue<ActorData>();
            AddTask(startPageData);
        }

        public ScraperController(Data startPageData, int maxActorPages, int maxMovePages)
        {
            _startPageData = startPageData;
            SetMaxActorPages(maxActorPages);
            SetMaxMoviePages(maxMovePages);
            
            _movieInputs = new Queue<MovieData>();
            _actorInputs = new Queue<ActorData>();
            AddTask(startPageData);
        }
        
        /// <inheritdoc/>
        public Graph BeginScraping()
        {
            //Error checks
            UrlUtilities.ThowExeptionIfInvalid(_startPageData.GetUrl());

            Graph g = new Graph();

            //Continue to loop through queue till reach max
            bool reachedMaxActors = false;
            bool reachedMaxMovies = false;
            bool reachedMaximum = false;

            int actorsParsed = 0;
            int moviesParsed = 0;

            while (!reachedMaximum)
            {
                while (!reachedMaxActors)
                {
                    //Try to get next elem
                    ActorData data;
                    try
                    {
                        data = _actorInputs.Dequeue();
                    } catch (InvalidOperationException e)
                    {
                        //no actor inputs
                        break;
                    }

                    //Populate actor data
                    try
                    {
                        data = ActorScrapingUtilities.PopulateActorData(data,
                            UrlUtilities.LoadPageToDoc(data.GetUrl()),
                            this);
                        actorsParsed++;
                    }
                    catch (ArgumentException e)
                    {
                        continue;
                    }

                    if (data != null)
                    {
                        //Add data to graph
                        ActorNode node = new ActorNode(data);
                        g.AddNode(node);
                    }

                    //Check if reached maximum
                    if (actorsParsed >= _maxActorPages)
                    {
                        reachedMaxActors = true;
                    }
                }

                while (!reachedMaxMovies)
                {
                    //Try to get next elem
                    MovieData data;
                    try
                    {
                        data = _movieInputs.Dequeue();
                    }
                    catch (InvalidOperationException e)
                    {
                        //no movie inputs
                        break;
                    }

                    try
                    {
                        //Populate movie data
                        data = MovieScrapingUtilities.PopulateMovieData(data,
                            UrlUtilities.LoadPageToDoc(data.GetUrl()),
                            this);
                        moviesParsed++;
                    }
                    catch (ArgumentException e)
                    {
                        continue;
                    }

                    if (data != null)    //Check if successfully parsed
                    {
                        //Add data to graph
                        MovieNode node = new MovieNode(data);
                        g.AddNode(node);
                    }
                    
                    
                    //Check if reached maximum
                    if (moviesParsed >= _maxMovePages)
                    {
                        reachedMaxMovies = true;
                    }
                }

                reachedMaximum = reachedMaxActors && reachedMaxMovies;
            }
            
            //Populate actor nodes with total gross
            g.PopulateTotalGross();

            return g;
        }
        
        /// <inheritdoc/>
        public Data GetStartPage()
        {
            return _startPageData;
        }

        /// <inheritdoc/>
        public void AddTask(Data pageData)
        {
            if (pageData == null)
                throw new ArgumentNullException(nameof(pageData), "InputPage cannot be null.");

            if (pageData is ActorData actorData)
            {
                _actorInputs.Enqueue(actorData);
            }
            else if (pageData is MovieData movieData)
            {
                _movieInputs.Enqueue(movieData);
            }
            else
            {
                throw new ArgumentException("Cannot cast argument to ActorData nor PageData");
            }
        }

        /// <inheritdoc/>
        public void SetMaxActorPages(int maxActorPages)
        {
            if (maxActorPages < 0)
            {
                throw new ArgumentException("Maximum actor pages to parse must be 0 or greater.");
            }
            _maxActorPages = maxActorPages;
        }

        /// <inheritdoc/>
        public void SetMaxMoviePages(int maxMoviePages)
        {
            if (maxMoviePages < 0)
            {
                throw new ArgumentException("Maximum actor pages to parse must be 0 or greater.");
            }
            _maxMovePages = maxMoviePages;
        }
        
    }
}