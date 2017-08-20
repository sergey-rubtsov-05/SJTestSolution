using System;
using System.Collections.Generic;
using AutoMapper;

namespace WebApp.Engine.Maps
{
    public static class AutoMapperConfigurator
    {
        private static readonly object Lock = new object();

        public static MapperConfiguration Configure()
        {
            lock (Lock)
            {
                IList<IAutoMapperTypeConfigurator> configurators = new List<IAutoMapperTypeConfigurator>();
                configurators.Add(new MessageConfigurator());
                Action<IMapperConfigurationExpression> configure = config =>
                {
                    foreach (var configurator in configurators)
                        configurator.Configure(config);
                };
                var configuration = new MapperConfiguration(configure);
                return configuration;
            }
        }
    }
}