using DevJobs.Application;
using DevJobs.Domain.Entities;
using DevJobs.Infrastructure.Features.WebScraping;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Text.RegularExpressions;

namespace DevJobs.Infrastructure.Features.JobParsing;

public abstract class JobsParser
{
    private readonly ILogger<JobsParser> _logger;
    private readonly IApplicationUnitOfWork _unitOfWork;

    public JobsParser(ILogger<JobsParser> logger, IApplicationUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public abstract Task ParseAllJobsAsync();

    protected abstract JobPost GetDetailOfAnItem(string itemLink);

    protected abstract IHtmlPageDownloader GetPageDownloader();

    private string FilterCompanyName(string originalName)
    {
        return originalName.Replace("Ltd", "").Replace("ltd", "").Replace("LTD", "")
            .Replace("Limited", "").Replace("limited", "").Replace("LIMITED", "").Trim('.').Trim();
    }

    protected Organization GetCompany(string name)
    {
        name = FilterCompanyName(name);
        var company = _unitOfWork.JobPostRepository.GetCompany(name);
        return company;
    }

    private protected async Task InitializeDataAsync()
    {
        if (await _unitOfWork.ExperienceMappingRepository.GetCountAsync(x => true) <= 7)
        {
            await InitExperienceDataAsync();
        }

        if (await _unitOfWork.TechnologyRepository.GetCountAsync(x => true) == 0)
        {
            await InitTechnologyAsync();
        }

        if (await _unitOfWork.TrackMappingRepository.GetCountAsync(x => true) == 0)
        {
            await InitTrackDataAsync();
        }
    }

    private async Task InitExperienceDataAsync()
    {
        await _unitOfWork.ExperienceMappingRepository.RemoveAsync(x => true);

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Jr.",
            Level = ExperienceLevel.EntryLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Junior",
            Level = ExperienceLevel.EntryLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Sr.",
            Level = ExperienceLevel.MidLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Senior",
            Level = ExperienceLevel.MidLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Lead",
            Level = ExperienceLevel.TechLead
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Chief",
            Level = ExperienceLevel.CLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "CTO",
            Level = ExperienceLevel.CLevel
        });

        await _unitOfWork.ExperienceMappingRepository.AddAsync(new ExperienceMapping()
        {
            Keyword = "Intern",
            Level = ExperienceLevel.Intern
        });

        await _unitOfWork.SaveAsync();
    }

    private async Task InitTrackDataAsync()
    {
        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = ".net",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Asp",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "C#",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "DotNet",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Dot Net",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Sharepoint",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Windows",
            JobTrack = Track.DotNet
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "PHP",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Laravel",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "CakePHP",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Umbraco",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Wordpress",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Magento",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Drupal",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Joomla",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Wp",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Codeigniter",
            JobTrack = Track.PHP
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "SQA",
            JobTrack = Track.SQA
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "QA",
            JobTrack = Track.SQA
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Quality",
            JobTrack = Track.SQA
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Java",
            JobTrack = Track.Java
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "iOS",
            JobTrack = Track.iOS
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Objective-C",
            JobTrack = Track.iOS
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Android",
            JobTrack = Track.Android
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "UX",
            JobTrack = Track.UX
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "System",
            JobTrack = Track.SystemAdmin
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "DBA",
            JobTrack = Track.DBA
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Database",
            JobTrack = Track.DBA
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Python",
            JobTrack = Track.Python
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Ruby",
            JobTrack = Track.Ruby
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Perl",
            JobTrack = Track.Perl
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Graphic",
            JobTrack = Track.Graphic
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "Network",
            JobTrack = Track.Network
        });

        await _unitOfWork.TrackMappingRepository.AddAsync(new TrackMapping()
        {
            Keyword = "C++",
            JobTrack = Track.Cpp
        });

        await _unitOfWork.SaveAsync();
    }

    private async Task InitTechnologyAsync()
    {
        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Java",
            TechnologyTrack = Track.Java
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "iOS",
            TechnologyTrack = Track.iOS
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Android",
            TechnologyTrack = Track.Android
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "MySql",
            TechnologyTrack = Track.DBA
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Oracle",
            TechnologyTrack = Track.DBA
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Python",
            TechnologyTrack = Track.Python
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Ruby",
            TechnologyTrack = Track.Ruby
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "C++",
            TechnologyTrack = Track.Cpp
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Perl",
            TechnologyTrack = Track.Perl
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "PHP",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Wp",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Codeigniter",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Joomla",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Magento",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Drupal",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Wordpress",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Umbraco",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "CakePHP",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Laravel",
            TechnologyTrack = Track.PHP
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Sharepoint",
            TechnologyTrack = Track.DotNet
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "C#",
            TechnologyTrack = Track.DotNet
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "Asp",
            TechnologyTrack = Track.DotNet
        });

        await _unitOfWork.TechnologyRepository.AddAsync(new Technology()
        {
            Name = "SQL Server",
            TechnologyTrack = Track.DotNet
        });

        await _unitOfWork.SaveAsync();
    }

    public List<JobAnalysis> GetJobAnalysis(JobPost post)
    {
        JobAnalysis analysis = new();
        List<JobAnalysis> analysisList = [];
        analysis.Post = post;
        analysis.JobPostID = post.Id;

        SetExperience(post, analysis);
        SetTrack(post, analysisList, analysis);

        return analysisList;
    }

    private void SetTechnology(JobPost post, List<JobTechnology> technologies)
    {
        var technology = _unitOfWork.TechnologyRepository.GetAll();

        string technologyRegex = string.Join("|",
            (from t in technology
             select t.Name).ToArray());

        technologyRegex = technologyRegex.Replace("+", @"\+");

        MatchCollection titleMatches = null;
        try
        {
            titleMatches = Regex.Matches(post.JobTitle ?? string.Empty, technologyRegex, RegexOptions.IgnoreCase);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error in Regex when JobTitle matching:{ex.Message}");
        }

        MatchCollection descriptionMatches = null;
        try
        {
            descriptionMatches = Regex.Matches(post.JobDescription ?? string.Empty, technologyRegex, RegexOptions.IgnoreCase);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error in Regex when description matching:{ex.Message}");
        }

        MatchCollection experienceRequirementsMatches = null;
        try
        {
            experienceRequirementsMatches = Regex.Matches(post.ExperienceRequirements ?? string.Empty, technologyRegex, RegexOptions.IgnoreCase);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error in Regex when ExperienceRequirements matching:{ex.Message}");
        }

        MatchCollection additionalJobRequirementsMatches = null;
        try
        {
            additionalJobRequirementsMatches = Regex.Matches(post.AdditionalJobRequirements ?? string.Empty, technologyRegex, RegexOptions.IgnoreCase);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error in Regex when AdditionalJobRequirements matching:{ex.Message}");
        }

        try
        {
            foreach (Match match in titleMatches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (!technologies.Any(x => x.Name.Equals(capture.Value, StringComparison.InvariantCultureIgnoreCase)))
                        technologies.Add(new JobTechnology() { Name = capture.Value });
                }
            }
        }
        catch
        {
            technologies.Add(new JobTechnology());
        }

        try
        {
            foreach (Match match in descriptionMatches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (!technologies.Any(x => x.Name.Equals(capture.Value, StringComparison.InvariantCultureIgnoreCase)))
                        technologies.Add(new JobTechnology() { Name = capture.Value });
                }
            }
        }
        catch
        {
            technologies.Add(new JobTechnology());
        }

        try
        {
            foreach (Match match in experienceRequirementsMatches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (!technologies.Any(x => x.Name.Equals(capture.Value, StringComparison.InvariantCultureIgnoreCase)))
                        technologies.Add(new JobTechnology() { Name = capture.Value });
                }
            }
        }
        catch
        {
            technologies.Add(new JobTechnology());
        }

        try
        {
            foreach (Match match in additionalJobRequirementsMatches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (!technologies.Any(x => x.Name.Equals(capture.Value, StringComparison.InvariantCultureIgnoreCase)))
                        technologies.Add(new JobTechnology() { Name = capture.Value });
                }
            }
        }
        catch
        {
            technologies.Add(new JobTechnology());
        }
    }

    private void SetTrack(JobPost post, List<JobAnalysis> analysisList, JobAnalysis analyse)
    {
        var trackMappings = _unitOfWork.TrackMappingRepository.GetAll();

        string dotNetMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.DotNet
             select t.Keyword).ToArray());

        dotNetMappingRegex = dotNetMappingRegex.Replace(".", "\\.");

        string androidMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Android
             select t.Keyword).ToArray());

        string cppMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Cpp
             select t.Keyword).ToArray());

        cppMappingRegex = cppMappingRegex.Replace("+", @"\+");

        string dbaMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.DBA
             select t.Keyword).ToArray());

        dbaMappingRegex = dbaMappingRegex.Replace("DBA", "\\bDBA");

        string graphicMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Graphic
             select t.Keyword).ToArray());

        string iOSMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.iOS
             select t.Keyword).ToArray());

        string javaMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Java
             select t.Keyword).ToArray());

        javaMappingRegex = javaMappingRegex.Replace("Java", "Java\\b");

        string networkMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Network
             select t.Keyword).ToArray());

        string perlMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Perl
             select t.Keyword).ToArray());

        string phpMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.PHP
             select t.Keyword).ToArray());

        string pythonMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Python
             select t.Keyword).ToArray());

        string rubyMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.Ruby
             select t.Keyword).ToArray());

        string sqaMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.SQA
             select t.Keyword).ToArray());

        sqaMappingRegex = sqaMappingRegex.Replace("Quality", "Software Quality Assurance");

        string systemAdminMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.SystemAdmin
             select t.Keyword).ToArray());

        string uxMappingRegex = string.Join("|",
            (from t in trackMappings
             where t.JobTrack == Track.UX
             select t.Keyword).ToArray());

        if (Regex.IsMatch(post.JobTitle ?? string.Empty, dotNetMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.DotNet
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, androidMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Android
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, cppMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Cpp
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, dbaMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.DBA
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, graphicMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Graphic
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, iOSMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.iOS
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, javaMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Java
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, networkMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Network
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, perlMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Perl
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, phpMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.PHP
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, pythonMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Python
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, rubyMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.Ruby
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, sqaMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.SQA
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, systemAdminMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                Experience = analyse.Experience,
                JobTrack = Track.SystemAdmin
            };
            analysisList.Add(analysis);
        }
        if (Regex.IsMatch(post.JobTitle ?? string.Empty, uxMappingRegex, RegexOptions.IgnoreCase))
        {
            List<JobTechnology> technologies = [];
            SetTechnology(post, technologies);
            JobAnalysis analysis = new()
            {
                JobPostID = analyse.JobPostID,
                Technologies = technologies,
                JobTrack = Track.UX,
                Experience = analyse.Experience
            };
            analysisList.Add(analysis);
        }

        try
        {
            if (analysisList.Count == 0)
            {
                List<JobTechnology> technologies = [];
                SetTechnology(post, technologies);
                JobAnalysis analysis = new()
                {
                    JobPostID = analyse.JobPostID,
                    Experience = analyse.Experience,
                    Technologies = technologies
                };

                if (Regex.IsMatch(post.JobDescription ?? string.Empty, dotNetMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, dotNetMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, dotNetMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.DotNet;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, androidMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, androidMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, androidMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Android;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, cppMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, cppMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, cppMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Cpp;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, dbaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, dbaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, dbaMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.DBA;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, graphicMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, graphicMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, graphicMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Graphic;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, iOSMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, iOSMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, iOSMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.iOS;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, javaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, javaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, javaMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Java;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, networkMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, networkMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, networkMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Network;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, perlMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, perlMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, perlMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Perl;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, phpMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, phpMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, phpMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.PHP;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, pythonMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, pythonMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, pythonMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Python;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, rubyMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, rubyMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, rubyMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.Ruby;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, sqaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, sqaMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, sqaMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.SQA;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, systemAdminMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, systemAdminMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, systemAdminMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.SystemAdmin;
                else if (Regex.IsMatch(post.JobDescription ?? string.Empty, uxMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.AdditionalJobRequirements ?? string.Empty, uxMappingRegex, RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(post.ExperienceRequirements ?? string.Empty, uxMappingRegex, RegexOptions.IgnoreCase))
                    analysis.JobTrack = Track.UX;

                analysisList.Add(analysis);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Set Track failed {ex.Message}");
        }

        if (analysisList.Count == 0)
        {
            List<JobTechnology> technologies = new List<JobTechnology>();
            SetTechnology(post, technologies);
            JobAnalysis analysis = new();
            analysis.JobPostID = analyse.JobPostID;
            analysis.Experience = analyse.Experience;
            analysis.Technologies = technologies;
            analysis.JobTrack = Track.Other;
            analysisList.Add(analysis);
        }
    }

    private void SetExperience(JobPost post, JobAnalysis analysis)
    {
        var experienceMappings = _unitOfWork.ExperienceMappingRepository.GetAll();

        string entryLevelMappingRegex = string.Join("|",
            (from e in experienceMappings
             where e.Level == ExperienceLevel.EntryLevel
             select e.Keyword).ToArray());

        string midLevelMappingRegex = string.Join("|",
            (from e in experienceMappings
             where e.Level == ExperienceLevel.MidLevel
             select e.Keyword).ToArray());

        string cLevelMappingRegex = string.Join("|",
            (from e in experienceMappings
             where e.Level == ExperienceLevel.CLevel
             select e.Keyword).ToArray());

        string internMappingRegex = string.Join("|",
            (from e in experienceMappings
             where e.Level == ExperienceLevel.Intern
             select e.Keyword).ToArray());

        string leadMappingRegex = string.Join("|",
            (from e in experienceMappings
             where e.Level == ExperienceLevel.TechLead
             select e.Keyword).ToArray());

        if (Regex.IsMatch(post.JobTitle ?? string.Empty, entryLevelMappingRegex, RegexOptions.IgnoreCase))
            analysis.Experience = ExperienceLevel.EntryLevel;
        else if (Regex.IsMatch(post.JobTitle ?? string.Empty, midLevelMappingRegex, RegexOptions.IgnoreCase))
            analysis.Experience = ExperienceLevel.MidLevel;
        else if (Regex.IsMatch(post.JobTitle ?? string.Empty, cLevelMappingRegex, RegexOptions.IgnoreCase))
            analysis.Experience = ExperienceLevel.CLevel;
        else if (Regex.IsMatch(post.JobTitle ?? string.Empty, leadMappingRegex, RegexOptions.IgnoreCase))
            analysis.Experience = ExperienceLevel.TechLead;
        else if (Regex.IsMatch(post.JobTitle ?? string.Empty, internMappingRegex, RegexOptions.IgnoreCase))
            analysis.Experience = ExperienceLevel.Intern;

        if (analysis.Experience == ExperienceLevel.NotSet)
        {
            if (post.ExperienceMin < 1)
                analysis.Experience = ExperienceLevel.EntryLevel;
            else if (post.ExperienceMin < 3)
                analysis.Experience = ExperienceLevel.MidLevel;
            else if (post.ExperienceMin < 5)
                analysis.Experience = ExperienceLevel.TechLead;
            else if (post.ExperienceMin >= 5)
                analysis.Experience = ExperienceLevel.CLevel;
        }
    }

    public void RefreshJobAnalysis()     /// inspect later
    {
        var jobs = _unitOfWork.JobPostRepository.GetAll();

        foreach (var job in jobs)
        {
            JobAnalysis analysis = new JobAnalysis();
            List<JobAnalysis> analysisList = new List<JobAnalysis>();
            analysis.Post = job;
            analysis.JobPostID = job.Id;

            SetExperience(job, analysis);
            SetTrack(job, analysisList, analysis);

            job.Analysis = analysisList;

            _unitOfWork.JobPostRepository.Edit(job);
        }

        _unitOfWork.Save();
    }

    protected async Task ParseJobsAsync(string url, string selector)
    {
        try
        {
            IHtmlPageDownloader downloader = GetPageDownloader();
            var listDoc = downloader.GetDocumentByLink(url);

            if (listDoc is null)
                return;

            foreach (var listNode in listDoc.QuerySelectorAll(selector))
            {
                try
                {
                    var itemLink = listNode.GetAttributeValue("href", "");
                    JobPost post = GetDetailOfAnItem(itemLink);

                    if (post is not null)
                    {
                        post.Id = Guid.NewGuid();
                        List<JobAnalysis> analysisList = GetJobAnalysis(post);

                        //Disclaimer: if JobTitle, Company.Name, PublishedOn all together insert null in DB. 
                        //No jobpost'll insert again for this logic. If so, fixed imediately.
                        if (await _unitOfWork.JobPostRepository.GetCountAsync(x => x.Company.Name == post.Company.Name
                                && x.JobTitle == post.JobTitle
                                && x.PublishedOn == x.PublishedOn) == 0)
                        {
                            post.Analysis = analysisList;

                            await _unitOfWork.JobPostRepository.AddAsync(post);
                            await _unitOfWork.SaveAsync();
                        }
                    }

                    await Task.Delay(1000);
                }
                catch (WebException httpEx)
                {
                    throw new WebException();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred: {ErrorMessage}", ex.Message);
                }
            }
        }
        catch (WebException httpEx)
        {
            Log.Error("Error occurs: No internet connection Or Scraping websites may be down or wrong URL");
            throw new WebException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job parser failed to process or Wrong URL");
        }
    }
}