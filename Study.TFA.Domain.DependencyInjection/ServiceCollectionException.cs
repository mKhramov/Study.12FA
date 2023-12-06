using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Study.TFA.Domain.Authentication;
using Study.TFA.Domain.Authorization;
using Study.TFA.Domain.UseCases.CreateTopic;
using Study.TFA.Domain.UseCases.GetForums;

namespace Study.TFA.Domain.DependencyInjection
{
    public static class ServiceCollectionException
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            services
                .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                .AddScoped<IIntentionResolver, TopicIntentionResolver>();

            services
                .AddScoped<IIntentionManager, IntentionManager>()
                .AddScoped<IIdentityProvider, IdentityProvider>();

            services
                .AddValidatorsFromAssemblyContaining<Models.Forum>(includeInternalTypes: true);

            return services;
        }
    }
}
