using System;
using System.Collections.Generic;
using WebScraperAPI.Utilities;

namespace WebScraperAPI.Models.Graph
{
    /// <summary>
    /// Graph representing parsed data
    /// Adjacency list representation
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// List of nodes in graph
        /// Each node holds list of adjacent nodes
        /// </summary>
        private IList<Node> _nodes;

        public Graph()
        {
            _nodes = new List<Node>();
        }

        /// <summary>
        /// Adds a new node to the graph
        /// Internally handles case when node already exists
        /// </summary>
        /// <param name="node">Node to add</param>
        public void AddNode(Node node)
        {
            Node dup = GraphUtilities.ExistsDuplicate(node, this);
            if (dup == null)
            {
                PopulateNode(node);
                _nodes.Add(node);
            }
            else
            {
                //Node already exists in graph
                PopulateNode(dup);
            }
        }

        /// <summary>
        /// Getter for nodes
        /// </summary>
        /// <returns>List of nodes in graph</returns>
        public IList<Node> GetNodes()
        {
            return _nodes;
        }
        
        /// <summary>
        /// Adds adjacent nodes and edges to param node
        /// Assumes node data populated correctly
        /// </summary>
        /// <param name="node">Node to populate</param>
        private void PopulateNode(Node node)
        {
            if (node is ActorNode actorNode)
            {
                var movieCollection = (actorNode.GetData() as ActorData)?.Movies;
                if (movieCollection != null)
                    foreach (MovieData movieData in movieCollection)
                    {
                        //Make movie node
                        MovieNode newNode = new MovieNode(movieData);
                        Node dup = GraphUtilities.ExistsDuplicate(newNode, this);
                        if (dup == null)
                        {
                            //New movie node doesn't exist in graph yet
                            AddNode(newNode);
                            actorNode.AddAdjNode(newNode);
                            actorNode.AddEdge(CreateEdge(actorNode, newNode));
                            //Accumulate total gross
                        }
                        else
                        {
                            //Movie node already exists
                            actorNode.AddAdjNode(dup);
                            actorNode.AddEdge(CreateEdge(actorNode, dup as MovieNode));
                            //Accumulate total gross
                        }
                    }
            }
            else if (node is MovieNode movieNode)
            {
                var actorCollection = (movieNode.GetData() as MovieData)?.Actors;
                if (actorCollection != null)
                    foreach (ActorData actorData in actorCollection)
                    {
                        //Make actor node
                        ActorNode newNode = new ActorNode(actorData);
                        Node dup = GraphUtilities.ExistsDuplicate(newNode, this);
                        if (dup == null)
                        {
                            //New actor doesn't exist in graph yet
                            AddNode(newNode);
                            movieNode.AddAdjNode(newNode);
                            movieNode.AddEdge(CreateEdge(newNode, movieNode));
                        }
                        else
                        {
                            //Actor node already exists
                            movieNode.AddAdjNode(dup);
                            movieNode.AddEdge(CreateEdge(dup as ActorNode, movieNode));
                        }
                    }
            }
            else
            {
                throw new InvalidCastException("Cannot cast node to ActorNode nor MovieNode");
            }
            
        }

        /// <summary>
        /// Creates a new graph edge
        /// </summary>
        /// <param name="actorNode">Actor node of edge</param>
        /// <param name="movieNode">Movie node of edge</param>
        /// <returns>New edge</returns>
        private Edge CreateEdge(ActorNode actorNode, MovieNode movieNode)
        {
            int weight;
            if (actorNode.GetData() is ActorData actorData)
            {
                if (actorData.Movies != null)
                {
                    weight = actorData.Movies.Count;
                }
                else
                {
                    weight = 0;
                }
            }
            else
            {
                throw new InvalidCastException("Cannot cast to actor node.");
            }
            
            return new Edge(movieNode, actorNode, weight);
        }
        
        /// <summary>
        /// Populates actor's total gross
        /// </summary>
        public void PopulateTotalGross()
        {
            foreach (Node node in _nodes)
            {
                if (node is ActorNode actorNode
                && actorNode.GetData() is ActorData actorData)
                {
                    foreach (Edge edge in node.GetEdges())
                    {
                        if (edge.GetMovieNode().GetData() is MovieData movieData)
                        {
                            actorData.TotalGross += movieData.Gross;
                        }
                    }
                }
            }
        }
    }
}