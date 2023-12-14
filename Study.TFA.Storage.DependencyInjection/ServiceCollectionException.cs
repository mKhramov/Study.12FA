using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;
using Study.TFA.Domain.UseCases.GetTopics;
using Study.TFA.Storage.Storages;

namespace Study.TFA.Storage.DependencyInjection
{
    public static class ServiceCollectionException
    {
        public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
        {
            services
                .AddScoped<IGetForumsStorage, GetForumsStorage>()
                .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
                .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
                .AddScoped<IGuidFactory, GuidFactory>()
                .AddScoped<IMomentProvider, MomentProvider>()
                .AddValidatorsFromAssemblyContaining<Domain.Models.Forum>()
                .AddDbContextPool<ForumDbContext>(options => options
                    .UseNpgsql(dbConnectionString));

            return services;
        }
    }
}
