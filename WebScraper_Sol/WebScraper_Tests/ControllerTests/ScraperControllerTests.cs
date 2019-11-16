using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WebScraper.Controllers;
using WebScraper.Models;
using WebScraper.Models.Graph;


namespace WebScraper_Tests.ControllerTests
{
    /// <summary>
    /// Unit tests for ScraperController class
    /// </summary>
    public class ScraperControllerTests
    {
        MovieData movieStart = new MovieData("https://en.wikipedia.org/wiki/How_to_Train_Your_Dragon_(film)");
        ActorData actorStart = new ActorData("https://en.wikipedia.org/wiki/Tom_Hiddleston");
        
        [Test]
        public void Constructor_ValidInput_Obj()
        {
            ScraperController sc = new ScraperController(movieStart);
            Assert.AreEqual(sc.GetStartPage(), movieStart);
            
            sc = new ScraperController(actorStart);
            Assert.AreEqual(sc.GetStartPage(), actorStart);

            sc = new ScraperController(movieStart, 10, 13);
            Assert.AreEqual(sc.GetStartPage(), movieStart);
        }

        [Test]
        public void Constructor_InvalidInput_Obj()
        {
            ScraperController sc;
            try
            {
                sc = new ScraperController(movieStart, -1, 5);
                Assert.Fail();
            }
            catch
            {
            }

            try
            {
                sc = new ScraperController(movieStart, 5, -3);
            }
            catch
            {
            }
        }

        [Test]
        public void BeginScraping_ValidInput_Graph()
        {
            ScraperController sc = new ScraperController(movieStart, 2, 2);
            Graph g = sc.BeginScraping();
            
            Assert.AreEqual(66, g.GetNodes().Count);
            ActorData data = g.GetNodes()[0].GetData() as ActorData;
            Assert.AreEqual("Jay Baruchel", data.Name);
            Assert.AreEqual(38, data.Movies.Count);
        }
    }
}