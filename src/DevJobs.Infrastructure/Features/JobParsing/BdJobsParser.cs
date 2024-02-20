using DevJobs.Application;
using DevJobs.Application.Features.JobParsing;
using DevJobs.Domain.Entities;
using DevJobs.Infrastructure.Features.WebScraping;
using DevJobs.Infrastructure.Utilities;
using DevJobs.Infrastructure.WebScraping;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DevJobs.Infrastructure.Features.JobParsing;

public class BdJobsParser : JobsParser, IBdJobsParserService
{
    private readonly IConfiguration _configuration;

    public BdJobsParser(ILogger<JobsParser> logger,
        IApplicationUnitOfWork unitOfWork,
        IConfiguration configuration) : base(logger, unitOfWork)
    {
        _configuration = configuration;
    }

    public override async Task ParseAllJobsAsync()
    {
        await InitializeDataAsync();

        var pageOneLink = _configuration.GetValue<string>("DataCollectorService:PageOneLink");
        var xPath = "//div[@id='topPagging']/ul/li[last()]/a";
        var lastPage = FilterLastPageNumber(pageOneLink, xPath);
        var allPostLink = _configuration.GetValue<string>("DataCollectorService:AllPostLink");

        for (var i = 1; i <= lastPage; i++)
        {
            await ParseJobsAsync(allPostLink + i, ".job-title-text a");
        }
    }

    private int FilterLastPageNumber(string url, string xPath)
    {
        try
        {
            var lastPage = 10;
            var downloader = GetPageDownloader();
            var listDoc = downloader.GetDocumentByLink(url);

            if (listDoc == null)
                return lastPage;

            var lastPageElement = listDoc.DocumentNode.SelectSingleNode(xPath)?.InnerHtml?.TrimStart('.');

            if (lastPageElement != null)
            {
                lastPage = int.Parse(lastPageElement);
            }

            return lastPage;
        }
        catch (WebException webEx)
        {
            Log.Error("Error occurs: No internet connection Or Scraping websites may be down");
            throw new WebException();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to scrap last page or scraping websites may be down");
            return 10;
        }
    }

    protected override IHtmlPageDownloader GetPageDownloader()
    {
        return new BdJobsHtmlPageDownloader();
    }

    private string GetPrefixedLink(string url)
    {
        return "http://jobs.bdjobs.com/" + url;
    }

    private string RemoveSpecialCharsDescription(string input)
    {
        try
        {
            var plainText = input.Replace("<br>", " ")
                            .Replace("\n", "")
                            .Replace("N/A", "")
                            .Trim();
            return plainText;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurs when removing special character.");
        }

        return input;
    }

    private string RemoveSpecialChars(string input)
    {
        try
        {
            var plainText = input.Replace("<br>", " ")
                            .Replace("\r\n", "")
                            .Replace("\n", "")
                            .Trim();
            return plainText;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurs when removing special character.");
        }

        return input;
    }

    protected override JobPost GetDetailOfAnItem(string itemLink)
    {
        JobPost post = new();
        var downloader = GetPageDownloader();

        if (downloader is null)
            return null;

        var itemDoc = downloader?.GetDocumentByLink(GetPrefixedLink(itemLink));

        if (itemDoc is not null)
        {
            // skips scanned jobpost
            try
            {
                var jobTitle = itemDoc?.QuerySelector(".job-title")?.InnerText?.Trim();
                var companyName = itemDoc?.QuerySelector(".company-name")?.InnerText?.Trim();
                var imageElement = itemDoc?.DocumentNode?.SelectSingleNode(".//div[@class='image']/img")?.GetAttributes("src");

                if (imageElement is not null)
                {
                    foreach (var href in imageElement)
                    {
                        var hrefValue = href?.Value?.Trim();
                        if (hrefValue is not null)
                        {
                            if (hrefValue.Contains("scannedjobads") && jobTitle is not null && companyName is not null)
                                return null;
                            else
                                Log.Error("May be JobTitle or Company.Name QuerySelector changed in Bd Jobs. Fix ASAP!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when skipping scanned jobpost.");
            }

            // Source, CreatedOn
            post.Source = "www.bdjobs.com";
            post.CreatedOn = DateTime.UtcNow;

            // JobTitle
            try
            {

                post.JobTitle = itemDoc?.QuerySelector(".jtitle")?.InnerText?.Trim();
                if (post.JobTitle is null)
                    Log.Error("May be JobTitle QuerySelector changed in Bd Jobs.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing JobTitle.");
            }

            // Company.Name
            try
            {
                var companyName = itemDoc?.QuerySelector(".cname")?.InnerText?.Trim();
                if (companyName is not null)
                {
                    post.Company = GetCompany(companyName);
                }
                else
                {
                    Log.Error("May be company name querySelector changed in BD Jobs");
                    post.Company = new Organization();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error parsing when parsing Company.Name.");
                post.Company = new Organization();
            }

            // NumberOfVacancies, AgeMin, AgeMax, JobLocation, SalaryMin, ExperienceMin, ExperienceMax, PublishedOn
            try
            {
                var summaryItems = itemDoc?.QuerySelectorAll(".summery__items li");
                if (summaryItems is not null)
                {
                    if (summaryItems.Count > 0)
                    {
                        foreach (var summaryItem in summaryItems)
                        {
                            try
                            {
                                var text = summaryItem?.InnerText?.Trim()?.ToLower();
                                if (!text.IsNullOrEmpty())
                                {
                                    var itemValue = text.Split(new[] { ": " }, StringSplitOptions.None)[1];

                                    try
                                    {
                                        if (text.Contains("vacancy"))
                                        {
                                            if (text.Contains("--"))
                                                post.NumberOfVacancies = itemValue.Trim().ParseSafe();
                                            else if (text.Contains("-"))
                                                post.NumberOfVacancies = itemValue.Split('-')[1].Trim().ParseSafe();
                                            else
                                                post.NumberOfVacancies = itemValue.ParseSafe();

                                            if (post.NumberOfVacancies == 0)
                                                post.NumberOfVacancies = 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping NumberOfVacancies property.");
                                        post.NumberOfVacancies = 1;
                                    }

                                    try
                                    {
                                        if (text.Contains("age"))
                                        {
                                            itemValue = itemValue.Replace("years", "");
                                            itemValue = itemValue.Replace("age", "");

                                            if (itemValue.Contains("at most"))
                                                post.AgeMax = itemValue.Replace("at most", "").ParseSafe();
                                            else if (itemValue.Contains("at least"))
                                                post.AgeMin = itemValue.Replace("at least", "").ParseSafe();
                                            else
                                            {
                                                itemValue = itemValue.Replace(" to ", " ");
                                                try
                                                {
                                                    string[] ageValues = itemValue.Split(new[] { ' ' });
                                                    post.AgeMin = ageValues[1].ParseSafe();
                                                    post.AgeMax = ageValues[2].ParseSafe();
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping AgeMin, AgeMax property.");
                                    }

                                    try
                                    {
                                        if (text.Trim().Contains("location"))
                                        {
                                            post.JobLocation = itemValue.Trim();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping JobLocation property.");
                                    }

                                    try
                                    {
                                        if (text.Contains("salary"))
                                        {
                                            var salaryText = itemValue;

                                            if (salaryText is not "negotiable")
                                            {
                                                var regex = new Regex(@"tk\.\s*(.*?)\s*\(monthly\)");
                                                var match = regex.Match(salaryText);

                                                if (match.Success)
                                                {
                                                    salaryText = match.Groups[1].Value.Trim();

                                                    if (salaryText.Contains("-"))
                                                    {
                                                        var salaryValues = salaryText.Split(new[] { " - " }, StringSplitOptions.None);
                                                        post.SalaryMin = salaryValues[0].ParseSafe();
                                                        post.SalaryMax = salaryValues[1].ParseSafe();
                                                    }
                                                    else
                                                    {
                                                        post.SalaryMin = salaryText.ParseSafe();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping SalaryMin, SalaryMax property.");
                                    }

                                    try
                                    {
                                        if (text.Contains("experience"))
                                        {
                                            text = text.ToLower().Trim();
                                            if (text.ToLower().Contains("at least"))
                                            {
                                                //itemValue = itemValue.Replace("year(s)", "").Replace("At least", "");
                                                itemValue = itemValue.Replace("years", "").Replace("at least", "");
                                                itemValue = itemValue.Replace("year", "").Replace("at least", "");
                                                post.ExperienceMin = itemValue.Trim().ParseSafe();
                                            }
                                            else if (text.Contains("at most"))
                                            {
                                                //itemValue = itemValue.Replace("year(s)", "").Replace("At most", "");
                                                itemValue = itemValue.Replace("years", "").Replace("at most", "");
                                                itemValue = itemValue.Replace("year", "").Replace("at most", "");
                                                post.ExperienceMax = itemValue.Trim().ParseSafe();
                                            }
                                            else if (!text.ToLower().Contains("&nbsp;na"))
                                            {
                                                //itemValue = itemValue.Replace("year(s)", "");
                                                itemValue = itemValue.Replace("years", "").Replace("year", "");
                                                itemValue = itemValue.Replace(" to ", " ");
                                                string[] experienceValues = itemValue.Split(new[] { ' ' });
                                                try
                                                {
                                                    post.ExperienceMin = experienceValues[0].ParseSafe();
                                                    post.ExperienceMax = experienceValues[1].ParseSafe();
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping ExperienceMin, ExperienceMax property.");
                                    }

                                    try
                                    {
                                        if (text.Contains("published"))
                                        {
                                            post.PublishedOn = DateTime.Parse(itemValue);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, "Error occurs when scrapping PublishedOn property.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Unknown error occurring when parsing NumberOfVacancies, AgeMin, AgeMax, JobLocation, SalaryMin, ExperienceMin, ExperienceMax, PublishedOn");
                            }
                        }
                    }
                    else
                    {
                        Log.Error("May be summary( NumberOfVacancies, AgeMin, AgeMax, JobLocation, SalaryMin, ExperienceMin, ExperienceMax, PublishedOn ) QuerySelector changed in Bd Jobs.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing summary( NumberOfVacancies, AgeMin, AgeMax, JobLocation, SalaryMin, ExperienceMin, ExperienceMax, PublishedOn ) QuerySelector changed in Bd Jobs.");
            }

            // OtherBenifits
            try
            {
                var otherBenefits = itemDoc?.DocumentNode?.SelectNodes("//*[@id='benefits']");
                if (otherBenefits is not null)
                {
                    try
                    {
                        foreach (var otherBenefit in otherBenefits)
                        {
                            if (otherBenefit.InnerText.Contains("Compensation & Other Benefit") ||
                                otherBenefit.InnerText.Contains("Other Benefit"))
                            {
                                var ulElement = otherBenefit.NextSibling.NextSibling;
                                if (ulElement is HtmlNode ulNode)
                                {
                                    var liElements = ulNode?.SelectNodes(".//li");
                                    if (liElements != null && liElements.Any())
                                    {
                                        post.OtherBenefits = string.Join("\r\n", from i in liElements select RemoveSpecialChars(i.InnerText.Trim()));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error occurs when scraping OtherBenefits property");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing OtherBenefits.");
            }

            // JobDescription
            try
            {
                var jobResponsibilitiesElements = itemDoc?.DocumentNode?.SelectNodes("//*[@id='job_resp']");
                if (jobResponsibilitiesElements is not null)
                {
                    try
                    {
                        string jobDescriptionFinal = null;
                        foreach (var jobElement in jobResponsibilitiesElements)
                        {
                            var jobDescription = RemoveSpecialCharsDescription(jobElement.ParentNode.InnerText.Replace("Responsibilities & Context", "").Trim());
                            var jobDescriptionArray = Regex.Split(jobDescription, @"\r?\n|\r");

                            foreach (var str in jobDescriptionArray)
                            {
                                if (str.Any(char.IsLetter))
                                {
                                    jobDescriptionFinal = jobDescriptionFinal + str?.Trim() + "\r\n";
                                }
                            }
                        }

                        post.JobDescription = jobDescriptionFinal?.TrimEnd('\r', '\n');
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error occurs when scraping JobDescription property");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing JobDescription.");
            }

            // EducationalRequirements, ExperienceRequirements, AdditionalJobRequirements
            try
            {
                var jobRequirements = itemDoc?.DocumentNode?.SelectNodes("//*[@id='req']");
                if (jobRequirements is not null)
                {
                    try
                    {
                        foreach (var jobRequirement in jobRequirements)
                        {
                            var innerText = jobRequirement.InnerText.Trim();
                            try
                            {
                                if (innerText.Contains("Education"))
                                {
                                    var ulElement = jobRequirement.NextSibling.NextSibling;
                                    if (ulElement is HtmlNode ulNode)
                                    {
                                        var liElements = ulNode.SelectNodes(".//li");
                                        if (liElements != null && liElements.Any())
                                        {
                                            post.EducationalRequirements = string.Join("\r\n", from i in liElements select RemoveSpecialChars(i.InnerText.Trim()));
                                        }
                                        else
                                        {
                                            var elementP = jobRequirement?.NextSibling?.NextSibling?.NextSibling?.NextSibling;
                                            if (elementP?.Name == "p")
                                            {
                                                var educationP = RemoveSpecialChars(elementP.InnerText.Trim());
                                                post.EducationalRequirements = educationP;
                                            }
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping ExperienceRequirements property");
                            }

                            try
                            {
                                if (innerText.Contains("Experience"))
                                {
                                    var ulElement = jobRequirement.NextSibling.NextSibling;
                                    if (ulElement is HtmlNode ulNode)
                                    {
                                        var liElements = ulNode?.SelectNodes(".//li");
                                        if (liElements != null && liElements.Any())
                                        {
                                            post.ExperienceRequirements = string.Join("\r\n", from i in liElements select RemoveSpecialChars(i.InnerText.Trim()));
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping ExperienceRequirements property");
                            }

                            try
                            {
                                if (innerText.Contains("Additional Requirement"))
                                {
                                    var ulElement = jobRequirement.NextSibling.NextSibling;
                                    if (ulElement is HtmlNode ulNode)
                                    {
                                        var liElements = ulNode?.SelectNodes(".//li");
                                        if (liElements != null && liElements.Any())
                                        {
                                            post.AdditionalJobRequirements = string.Join("\r\n", from i in liElements select RemoveSpecialChars(i.InnerText.Trim()));
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping AdditionalJobRequirements property");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Unknow error occurring when parsing EducationalRequirements or ExperienceRequirements or AdditionalJobRequirements.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing EducationalRequirements or ExperienceRequirements or AdditionalJobRequirements.");
            }

            // JobNature, Gender
            try
            {
                var cssClasses = itemDoc?.QuerySelectorAll(".col-sm-12.mb-3 h4");
                if (cssClasses is not null)
                {
                    if (cssClasses.Count != 0)
                    {
                        foreach (var cssClass in cssClasses)
                        {
                            try
                            {
                                var cssClassInner = cssClass.InnerText.Trim();
                                try
                                {
                                    if (cssClassInner.Contains("Employment Status"))
                                    {
                                        var associatedPTag = cssClass?.ParentNode?.SelectSingleNode("p");
                                        post.JobNature = associatedPTag?.InnerText?.Trim();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex, "Error occurs when scrapping JobNature property");
                                }

                                try
                                {
                                    if (cssClassInner.Contains("Gender"))
                                    {
                                        var associatedPTag = cssClass?.ParentNode?.SelectSingleNode("p");

                                        if ((associatedPTag is not null) && associatedPTag.InnerText.Trim().ToLower().Contains("only male"))
                                            post.Gender = GenderOption.Male;
                                        if ((associatedPTag is not null) && associatedPTag.InnerText.Trim().ToLower().Contains("only female"))
                                            post.Gender = GenderOption.Female;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex, "Error occurs when scrapping Gender property");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Failed to parse Gender or  JobNature or error processing CSS class '{cssClass.InnerText}': {ex.Message}", ex);
                            }
                        }
                    }
                    else
                    {
                        Log.Error("May be Gender and JobNature QuerySelector changed in Bd Jobs.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing Gender and JobNature");
            }

            // Address, BusinessType, Website
            try
            {
                var organizationItems = itemDoc?.QuerySelectorAll(".jobcontent.compinfo h5");

                if (organizationItems is not null)
                {
                    foreach (var organizationItem in organizationItems)
                    {
                        if (!string.IsNullOrEmpty(organizationItem?.InnerText))
                        {
                            try
                            {
                                if (organizationItem.InnerText.ToLower().Contains("address"))
                                {
                                    post.Company.Address = organizationItem?.NextSibling?.NextSibling?.InnerText;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping Company.Address property");
                            }

                            try
                            {
                                if (organizationItem.InnerText.ToLower().Contains("business"))
                                {
                                    post.Company.BusinessType = organizationItem?.NextSibling?.NextSibling?.InnerText;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping Company.BusinessType property");
                            }

                            try
                            {
                                if (organizationItem.InnerText.ToLower().Contains("website"))
                                {
                                    post.Company.Website = organizationItem?.NextSibling?.NextSibling?.InnerText;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error occurs when scrapping Company.Website property");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error occurring when parsing company Address or BusinessType or Website");
            }

            return post;
        }

        return null;
    }
}