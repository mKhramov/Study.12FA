using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Study.TFA.API.DependencyInjection;
using Study.TFA.API.Middlewares;
using Study.TFA.Domain.DependencyInjection;
using Study.TFA.Storage.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment)
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres") ?? string.Empty);

builder.Services.AddAutoMapper(options => options.AddMaps(Assembly.GetExecutingAssembly()));

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