using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Samples.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Feed()
        {
            var builder = new StringBuilder();
            var sw = new StringWriter(builder);

            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Async = true, Indent = true, OmitXmlDeclaration = true }))
            {
                var writer = new RssFeedWriter(xmlWriter);
                await writer.WriteTitle("Test Feed");
                await writer.WriteDescription("Test description of the feed");

                foreach (var item in Enumerable.Range(0, 9))
                {
                    var feedItem = new SyndicationItem();
                    feedItem.Title = $"Setting-{item}";
                    feedItem.Description = $"Value {item}";
                    await writer.Write(feedItem);
                }

                xmlWriter.Flush();
            }

            return new ContentResult { Content = builder.ToString(), ContentType = "application/rss+xml" };
        }
    }
}
