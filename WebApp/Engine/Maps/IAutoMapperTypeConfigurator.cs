using AutoMapper;

namespace WebApp.Engine.Maps
{
    public interface IAutoMapperTypeConfigurator
    {
        void Configure(IMapperConfigurationExpression configuration);
    }
}