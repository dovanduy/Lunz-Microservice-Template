using System;
using AutoMapper;
using Lunz.Microservice.OrderManagement.Models.Api;

namespace Lunz.Microservice.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // INFO: 在这里配置更多的 Mapping 关系。
            CreateMap<Core.Models.OrderManagement.OrderItem, OrderItem>();
            CreateMap<OrderManagement.CommandStack.Domain.Models.Order, OrderDetails>();
            CreateMap<OrderManagement.CommandStack.Domain.Models.OrderItem, OrderItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.EntityId));
            CreateMap<OrderManagement.QueryStack.Models.Order, OrderDetails>();
        }
    }
}
