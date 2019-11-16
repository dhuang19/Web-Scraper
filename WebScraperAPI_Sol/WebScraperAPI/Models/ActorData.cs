using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebScraperAPI.Models
{
    /// <summary>
    /// Actor data class
    /// </summary>
    public class ActorData : Data
    {
        /// <summary>
        /// Name of actor/actress
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// List of movies this actor worked on
        /// </summary>
        public ICollection<MovieData> Movies { get; set; }

        /// <summary>
        /// Total grossed for all movies he/she was in
        /// </summary>
        public long TotalGross { get; set; }
        
        /// <summary>
        /// Birthday of this actor
        /// </summary>
        public DateTime Birthday { get; set; }

        public ActorData(string url) : base(url) { }

        public ActorData(string url, string name) : base(url)
        {
            Name = name;
        }
        
    }
}