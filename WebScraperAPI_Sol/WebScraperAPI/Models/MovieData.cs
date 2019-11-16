using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebScraperAPI.Models
{
    /// <summary>
    /// Movie data class
    /// </summary>
    public class MovieData : Data
    {
        /// <summary>
        /// Movie title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// How much this movie grossed
        /// </summary>
        public long Gross { get; set; }
        
        /// <summary>
        /// List of actors who worked on this movie
        /// </summary>
        public ICollection<ActorData> Actors { get; set; }

        /// <summary>
        /// Year this movie aired
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        public MovieData(string url) : base(url) { }

        public MovieData(string url, string title) : base(url)
        {
            Title = title;
        }

    }
}