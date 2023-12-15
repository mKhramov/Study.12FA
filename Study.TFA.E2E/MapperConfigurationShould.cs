﻿using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Study.TFA.E2E
{
    public class MapperConfigurationShould : IClassFixture<ForumApiApplicationFactory>
    {
        private readonly ForumApiApplicationFactory factory;

        public MapperConfigurationShould(
            ForumApiApplicationFactory factory)                    
        {
            this.factory = factory;
        }

        [Fact]
        public void BeValid()
        {
            factory.Services.GetRequiredService<IMapper>()
                .ConfigurationProvider.Invoking(p => p.AssertConfigurationIsValid())
                .Should().NotThrow();
        }
    }
}
