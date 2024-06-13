using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;

namespace FEventopia.Services.Settings
{
    public class AutoMapperSetting : Profile
    {
        public AutoMapperSetting()
        {
            CreateMap<Account, AccountModel>().ForMember("Id", opt => opt.Ignore());
            CreateMap<AccountProcessModel, Account>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                                              .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                                              .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                                              .ForMember(dest => dest.CreditAmount, opt => opt.Ignore())
                                              .ForMember(dest => dest.Role, opt => opt.Ignore())
                                              .ForMember(dest => dest.DeleteFlag, opt => opt.Ignore())
                                              .ForMember(dest => dest.UserName, opt => opt.Ignore())
                                              .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                                              .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                                              .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                                              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                                              .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                                              .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                                              .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                                              .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                                              .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                                              .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                                              .ForMember(dest => dest.Id, opt => opt.Ignore())
                                              .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                                              .ForMember(dest => dest.Version, opt => opt.Ignore());
            CreateMap<Transaction, TransactionModel>().ReverseMap();
            CreateMap<Location, LocationModel>().ReverseMap();
            CreateMap<Location, LocationProcessModel>().ReverseMap();
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Event, EventOperatorModel>().ReverseMap();
            CreateMap<Event, EventProcessModel>().ReverseMap();
        }
    }
}
