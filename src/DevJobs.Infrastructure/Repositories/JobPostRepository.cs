using DevJobs.Domain.Entities;
using DevJobs.Domain.Repositories;
using DevSkill.Core.Utilities;
using DevSkill.Data;
using DevSkill.Extensions.Paginate.Contracts;
using DevSkill.Extensions.Paginate.Extensions;
using DevSkill.Extensions.Queryable;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevJobs.Infrastructure.Repositories
{
    public class JobPostRepository : Repository<JobPost, Guid>, IJobPostRepository
    {
        public JobPostRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public Organization GetCompany(string companyName)
        {
            Expression<Func<JobPost, bool>> filterExpression = jobPost => jobPost.Company.Name == companyName;

            var jobPost = Get(filter: filterExpression, include: q => q.Include(jp => jp.Company)).FirstOrDefault();

            if (jobPost is not null)
            {
                return jobPost.Company;
            }

            return new Organization { Name = companyName };
        }

        public async Task<JobPost?> GetJobPostAsync(Guid id)
        {
            var jobPost = Get(
                filter: j => j.Id == id, 
                include: j => j.Include(j => j.Company).
                Include(j => j.Analysis).ThenInclude(j => j.Technologies))
                .FirstOrDefault();

            return jobPost;
        }

        public async Task<IPaginate<JobPost>> GetPaginatedJobPostsAsync(SearchRequest request)
        {
            var totalCount = await GetCountAsync();
            var posts = Get(null, null, null);

            var result = await posts
                .FilterBy(request.Filters)
                .OrderBy(request.SortOrders)
                .ToPaginateAsync(request.PageIndex, request.PageSize, totalCount, 1);

            return result;
        }
    }
}