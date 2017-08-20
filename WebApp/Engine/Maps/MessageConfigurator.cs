using AutoMapper;
using DataModel.Dto;
using DataModel.Enities;

namespace WebApp.Engine.Maps
{
    public class MessageConfigurator : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<Message, MessageDto>();
            map.ForMember(x => x.Username, e => e.MapFrom(x => x.User.Name));
        }
    }
}