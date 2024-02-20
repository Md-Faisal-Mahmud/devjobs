using Serilog.Configuration;
using Serilog.Events;
using Serilog;
using System.Net;
using Serilog.Sinks.Email;

namespace DevJobs.DataCollectorService
{
    public static class EmailExtension
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm} [{Level}] <{MachineName}> {Message}{NewLine}{Exception}";

        public static LoggerConfiguration DataCollectorServiceEmail
            (
            this LoggerSinkConfiguration loggerConfiguration,
            CustomEmailConnectionInfo connectionInfo,
            string outputTemplate = DefaultOutputTemplate,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum
            )
        {
            return loggerConfiguration.Email
                (
                connectionInfo,
                outputTemplate,
                restrictedToMinimumLevel
                );
        }

        public class CustomEmailConnectionInfo : EmailConnectionInfo
        {
            public CustomEmailConnectionInfo()
            {
                NetworkCredentials = new NetworkCredential();
            }
        }
    }
}