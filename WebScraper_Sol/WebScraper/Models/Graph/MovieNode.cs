namespace WebScraper.Models.Graph
{
    /// <summary>
    /// Node containing Movie data
    /// </summary>
    public class MovieNode : Node
    {
        /// <summary>
        /// Movie this node represents
        /// </summary>
        private MovieData _movieData;

        public MovieNode(MovieData movieData) : base()
        {
            _movieData = movieData;
        }

        /// <inheritdoc/>
        public override Data GetData()
        {
            return _movieData;
        }
    }
}