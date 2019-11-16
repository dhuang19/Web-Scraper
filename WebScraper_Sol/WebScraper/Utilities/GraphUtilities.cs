using WebScraper.Models;
using WebScraper.Models.Graph;

namespace WebScraper.Utilities
{
    /// <summary>
    /// Utilities for graph
    /// </summary>
    public static class GraphUtilities
    {
        /// <summary>
        /// Determines if adding node param into graph would be duplicate
        /// </summary>
        /// <param name="node">Node to add</param>
        /// <param name="g">Existing graph</param>
        /// <returns>Duplicate node if it exists; null else</returns>
        public static Node ExistsDuplicate(Node node, Graph g)
        {
            foreach (Node n in g.GetNodes())
            {
                if (SameNode(node, n))
                {
                    return n;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines if two nodes represent the same data
        /// </summary>
        /// <param name="n1">Node 1</param>
        /// <param name="n2">Node 2</param>
        /// <returns></returns>
        private static bool SameNode(Node n1, Node n2)
        {
            if (n1 is ActorNode && n2 is ActorNode)
            {
                return ((n1.GetData() as ActorData)?.Name == (n2.GetData() as ActorData)?.Name);
            }

            if (n1 is MovieNode && n2 is MovieNode)
            {
                return ((n1.GetData() as MovieData)?.Title == (n2.GetData() as MovieData)?.Title);
            }

            return false;
        }
    }

}