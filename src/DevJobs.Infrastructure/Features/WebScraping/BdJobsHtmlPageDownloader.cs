using DevJobs.Infrastructure.Features.WebScraping;
using HtmlAgilityPack;

namespace DevJobs.Infrastructure.WebScraping
{
    public class BdJobsHtmlPageDownloader : IHtmlPageDownloader
    {
        public HtmlDocument GetDocumentByLink(string url)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            return doc;
        }
    }
}