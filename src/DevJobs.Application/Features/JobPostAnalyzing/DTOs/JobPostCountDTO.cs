using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Application.Features.JobPostAnalyzing.DTOs
{
    public class JobPostCountDTO
    {
        public string Date { get; set; } = string.Empty;
        public int JobCount { get; set; }
    }
}
