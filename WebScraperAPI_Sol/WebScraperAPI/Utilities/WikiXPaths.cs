namespace WebScraperAPI.Utilities
{
    /// <summary>
    /// Constant strings used to select nodes for particular properties
    /// </summary>
    public static class WikiXPaths
    {
        /// <summary>
        /// Base Url for Wikipedia targets
        /// </summary>
        public static string WikiBaseUrl = "https://en.wikipedia.org";
        
        /// <summary>
        /// Select nodes for Actor name
        /// </summary>
        public static string ActorScrapeName = ".//h1";
        
        /// <summary>
        /// Select nodes for Actor birthday
        /// </summary>
        public static string ActorScrapeBirthday = "//span[@class='bday']";

        /// <summary>
        /// Select node for baked in table with movies
        /// </summary>
        public static string ActorScrapeBakedMoviesTable = "//table[@class='wikitable plainrowheaders sortable']";

        public static string ActorScrapeBakedMoviesTableAlternative = "//table[@class='wikitable sortable']";

        /// <summary>
        /// Select nodes for baked in table entries
        /// </summary>
        public static string ActorScrapeBakedMoviesTableEntries = "//tbody//tr//td//i//a";
        
        /// <summary>
        /// Select nodes for Actor main film article
        /// </summary>
        public static string ActorScrapeFilmArticle = ".//div[text()='Main article: ']";

        /// <summary>
        /// Select first table node in main film article
        /// </summary>
        public static string ActorScrapeFilmArticleTable = "//table";
        
        /// <summary>
        /// Select nodes for film entries in Actor main film article
        /// </summary>
        public static string ActorScrapeFilmArticleEntries = "//tbody//tr//th//a";

        /// <summary>
        /// Select nodes for Movie title
        /// </summary>
        public static string MovieScrapeTitle = ".//h1//i";

        /// <summary>
        /// Select nodes for Movie gross
        /// </summary>
        public static string MovieScrapeGross = "//table[@class='infobox vevent']//tbody";

        /// <summary>
        /// Select node for Movie cast header
        /// </summary>
        public static string MovieScrapeCastHeader = ".//span[@id='Cast']";

    }
}