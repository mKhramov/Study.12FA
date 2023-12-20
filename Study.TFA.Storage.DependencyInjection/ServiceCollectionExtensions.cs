using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Study.TFA.Domain.UseCases.CreateForum;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;
using Study.TFA.Domain.UseCases.GetTopics;
using Study.TFA.Domain.UseCases.SignIn;
using Study.TFA.Domain.UseCases.SignOn;
using Study.TFA.Storage.Storages;
using System.Reflection;

namespace Study.TFA.Storage.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
        {
            services
                .AddScoped<ICreateForumStorage, CreateForumStorage>()
                .AddScoped<IGetForumsStorage, GetForumsStorage>()
                .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                .AddScoped<ISignInStorage, SignInStorage>()
                .AddScoped<ISignOnStorage, SignOnStorage>()
                .AddScoped<IGuidFactory, GuidFactory>()
                .AddScoped<IMomentProvider, MomentProvider>()
                .AddValidatorsFromAssemblyContaining<Domain.Models.Forum>()
                .AddDbContextPool<ForumDbContext>(options => options
                    .UseNpgsql(dbConnectionString));


            services.AddMemoryCache();
            services.AddAutoMapper(config => config
                .AddMaps(Assembly.GetAssembly(typeof(ForumDbContext))));

            return services;
        }
    }
}
