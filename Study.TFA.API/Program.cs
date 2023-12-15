using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;
using Study.TFA.API.Mapping;
using Study.TFA.API.Middlewares;
using Study.TFA.Domain.DependencyInjection;
using Study.TFA.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
builder.Services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithProperty("Application", "Study.TFA.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(Matching.FromSource("Microsoft"))
        .WriteTo.OpenSearch(
            nodeUris: builder.Configuration.GetConnectionString("Logs"),
            indexFormat: "forum-logs-{0:yyyy.MM.dd}"))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(Matching.FromSource("Microsoft"))
        .WriteTo.OpenSearch(
            nodeUris: builder.Configuration.GetConnectionString("Logs"),
            indexFormat: "forum-dbquery-logs-{0:yyyy.MM.dd}"))
    .WriteTo.Logger(lc => lc.WriteTo.Console())
    .CreateLogger()));


builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres") ?? string.Empty);

builder.Services.AddAutoMapper(options => options.AddProfile<ApiProfile>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program
{ 

}