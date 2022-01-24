
using AutoMapper;
using MobilePayAPI.Dtos;
using MobilePayAPI.Entities;

namespace MobilePayAPI.Profiles
{
    public class MerchantsProfile : Profile
    {
        public MerchantsProfile()
        {
            CreateMap<MerchantCreateDto, Merchant>();
            CreateMap<Merchant, MerchantReadDto>();
            CreateMap<MerchantCreateDto, MerchantReadDto>();
        }
    }
}