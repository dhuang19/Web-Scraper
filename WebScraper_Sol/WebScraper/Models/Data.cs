namespace WebScraper.Models
{
    public class Data
    {
        /// <summary>
        /// URL of input pages
        /// </summary>
        private string _url;

        public Data(string url)
        {
            //TODO: error check
            _url = url;
        }

        public string GetUrl()
        {
            return _url;
        }
        
    }
}