using AutoMapper;
using Domain;
using Application.DTOs;
using Application.Commands;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Money, decimal>().ConvertUsing(m => m.Value);
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountRequest, CreateAccountCommand>();
            CreateMap<DepositMoneyRequest, DepositMoneyCommand>();
            CreateMap<WithdrawMoneyRequest, WithdrawMoneyCommand>();
        }
    }
}