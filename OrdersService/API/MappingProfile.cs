using AutoMapper; 
using Domain;
using Application.DTOs;
using Application.Commands;

namespace API {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Money, decimal>()
                .ConvertUsing(m => m.Value);

            CreateMap<Order, OrderDto>()
                .ForCtorParam("id",        opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("userId",    opt => opt.MapFrom(src => src.UserId))
                .ForCtorParam("amount",    opt => opt.MapFrom(src => src.Amount))
                .ForCtorParam("description", opt => opt.MapFrom(src => src.Description))
                .ForCtorParam("status",    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForCtorParam("createdAt", opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<CreateOrderRequest, CreateOrderCommand>()
                .ForCtorParam("userId",     opt => opt.MapFrom(src => src.UserId))
                .ForCtorParam("amount",     opt => opt.MapFrom(src => src.Amount))
                .ForCtorParam("description", opt => opt.MapFrom(src => src.Description));
        }
    }
}