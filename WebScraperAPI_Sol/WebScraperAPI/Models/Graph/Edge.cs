namespace WebScraperAPI.Models.Graph
{
    /// <summary>
    /// Edge between a movie node and actor node
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Weight of edge based on the number of movies the actor has been in
        /// </summary>
        private int _weight;

        /// <summary>
        /// The movie node this edge is attached to
        /// </summary>
        private MovieNode _movieNode;

        /// <summary>
        /// The actor node this edge is attached to
        /// </summary>
        private ActorNode _actorNode;

        public Edge(MovieNode movieNode, ActorNode actorNode, int weight)
        {
            _movieNode = movieNode;
            _actorNode = actorNode;
            _weight = weight;
        }

        public MovieNode GetMovieNode()
        {
            return _movieNode;
        }
    }
}