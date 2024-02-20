using HtmlAgilityPack;
namespace DevJobs.Infrastructure.Features.WebScraping
{
    public interface IHtmlPageDownloader
    {
        HtmlDocument GetDocumentByLink(string url);
    }
}
