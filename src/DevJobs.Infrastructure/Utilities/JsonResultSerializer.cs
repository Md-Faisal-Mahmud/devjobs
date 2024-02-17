using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevJobs.Infrastructure.Utilities
{
    public static class JsonResultSerializer
    {
        public static async Task<string> SerializeAsync<T>(T value)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            return JsonSerializer.Serialize(value, options);
        }
    }
}
