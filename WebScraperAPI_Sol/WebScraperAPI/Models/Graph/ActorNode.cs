namespace WebScraperAPI.Models.Graph
{
    /// <summary>
    /// Node containing Actor data
    /// </summary>
    public class ActorNode : Node
    {
        /// <summary>
        /// Actor this node represents
        /// </summary>
        private ActorData _actorData;

        public ActorNode(ActorData actorData) : base()
        {
            _actorData = actorData;
        }

        /// <inheritdoc/>
        public override Data GetData()
        {
            return _actorData;
        }
    }
}