using System.Collections.Generic;

namespace WebScraper.Models.Graph
{
    /// <summary>
    /// Base node class
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// List of edges connected to this node
        /// Ordered by smallest to largest weight
        /// </summary>
        private IList<Edge> _edges;

        /// <summary>
        /// List of nodes adjacent to this node
        /// Makes graph an adjacency list representation
        /// </summary>
        private readonly IList<Node> _adjNodes;

        protected Node()
        {
            _edges = new List<Edge>();
            _adjNodes = new List<Node>();
        }

        /// <summary>
        /// Returns the data this node is holding
        /// </summary>
        /// <returns>Data</returns>
        public abstract Data GetData();

        public void AddAdjNode(Node node)
        {
            _adjNodes.Add(node);
        }

        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        public IList<Edge> GetEdges()
        {
            return _edges;
        }
    }
}