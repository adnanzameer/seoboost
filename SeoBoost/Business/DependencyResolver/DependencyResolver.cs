﻿using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using SeoBoost.Business.Url;

namespace SeoBoost.Business.DependencyResolver
{
    [InitializableModule]
    public class DependencyResolver : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            IServiceConfigurationProvider configurationProvider = context.Services;
            AddHybridServices(configurationProvider);
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private static void AddHybridServices(IServiceConfigurationProvider configurationProvider)
        {
            configurationProvider.AddHttpContextOrThreadScoped<IUrlService, UrlService>();
        }
    }
}
