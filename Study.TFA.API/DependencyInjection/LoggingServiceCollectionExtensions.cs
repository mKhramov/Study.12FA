using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Filters;

namespace Study.TFA.API.DependencyInjection
{
    internal static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddApiLogging(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            // Logging configuration
            services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "Study.TFA.API")
                .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.OpenSearch(
                        nodeUris: configuration.GetConnectionString("Logs"),
                        indexFormat: "forum-logs-{0:yyyy.MM.dd}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft"))
                    .WriteTo.OpenSearch(
                        nodeUris: configuration.GetConnectionString("Logs"),
                        indexFormat: "forum-dbquery-logs-{0:yyyy.MM.dd}"))
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()));

            return services;
        }
    }
}
