using DevSkill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevJobs.Domain.Entities
{
    public class DbLog : IEntity<int>
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Exception { get; set; }
        public string? LogEvent { get; set; }
        public string? TraceId { get; set; }
        public string? SpanId { get; set; }
    }
}
