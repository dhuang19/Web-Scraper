using WebScraper.Models;
using WebScraper.Models.Graph;

namespace WebScraper.Interfaces
{
    /// <summary>
    /// Interface for controller for the flow of scaping
    /// </summary>
    public interface ISraperController
    {
        /// <summary>
        /// Sets the maximum number of actor pages to parse
        /// </summary>
        /// <param name="maxActorPages">Max number of actor pages to parse</param>
        void SetMaxActorPages(int maxActorPages);

        /// <summary>
        /// Sets the maximum number of movie pages to parse
        /// </summary>
        /// <param name="maxMoviePages">Max number of movie pages to parse</param>
        void SetMaxMoviePages(int maxMoviePages);

        /// <summary>
        /// Getter for start page
        /// </summary>
        /// <returns>Start Page</returns>
        Data GetStartPage();

        /// <summary>
        /// Adds the input page to appropriate queue
        /// </summary>
        /// <param name="pageData">Input page to add</param>
        void AddTask(Data pageData);

        /// <summary>
        /// Begin scraping process after all configs set up
        /// </summary>
        /// <returns>Graph representing scraped data</returns>
        Graph BeginScraping();
    }
}