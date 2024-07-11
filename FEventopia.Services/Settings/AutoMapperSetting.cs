using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FEventopia.Services.Settings
{
    public class AutoMapperSetting : Profile
    {
        public AutoMapperSetting()
        {
            CreateMap<Account, AccountModel>().ReverseMap();
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

            CreateMap<Event, EventModel>().ForMember(dest => dest.EventDetail, opt => opt.MapFrom(src => src.EventDetail.Select(ed => new EventDetailModel
            {
                StartDate = ed.StartDate,
                EndDate = ed.EndDate,
                TicketForSaleInventory = ed.TicketForSaleInventory,
                StallForSaleInventory = ed.StallForSaleInventory,
                TicketPrice = ed.TicketPrice
            })));

            CreateMap<Event, EventOperatorModel>().ForMember(dest => dest.EventDetail, opt => opt.MapFrom(src => src.EventDetail.Select(ed => new EventDetailOperatorModel
            {
                StartDate = ed.StartDate,
                EndDate = ed.EndDate,
                TicketForSaleInventory = ed.TicketForSaleInventory,
                StallForSaleInventory = ed.StallForSaleInventory,
                TicketPrice = ed.TicketPrice,
                EstimateCost = ed.EstimateCost
            })));

            CreateMap<Event, EventOperatorModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Convert Event.Id to string
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.EventName))
                .ForMember(dest => dest.EventDescription, opt => opt.MapFrom(src => src.EventDescription))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Banner))
                .ForMember(dest => dest.InitialCapital, opt => opt.MapFrom(src => src.InitialCapital))
                .ForMember(dest => dest.SponsorCapital, opt => opt.MapFrom(src => src.SponsorCapital))
                .ForMember(dest => dest.TicketSaleIncome, opt => opt.MapFrom(src => src.TicketSaleIncome))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.EventDetail, opt => opt.MapFrom(src => src.EventDetail.Where(ed => !ed.DeleteFlag)));

            CreateMap<EventDetail, EventDetailOperatorModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Convert EventDetail.Id to string
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.TicketForSaleInventory, opt => opt.MapFrom(src => src.TicketForSaleInventory))
                .ForMember(dest => dest.StallForSaleInventory, opt => opt.MapFrom(src => src.StallForSaleInventory))
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                .ForMember(dest => dest.EstimateCost, opt => opt.MapFrom(src => src.EstimateCost))
                .ForMember(dest => dest.Location, opt => opt.UseDestinationValue());

            CreateMap<Event, EventModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Convert Event.Id to string
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.EventName))
                .ForMember(dest => dest.EventDescription, opt => opt.MapFrom(src => src.EventDescription))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Banner))
                .ForMember(dest => dest.EventDetail, opt => opt.MapFrom(src => src.EventDetail.Where(ed => !ed.DeleteFlag)));

            CreateMap<EventDetail, EventDetailModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Convert EventDetail.Id to string
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.TicketForSaleInventory, opt => opt.MapFrom(src => src.TicketForSaleInventory))
                .ForMember(dest => dest.StallForSaleInventory, opt => opt.MapFrom(src => src.StallForSaleInventory))
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.TicketPrice))
                .ForMember(dest => dest.Location, opt => opt.UseDestinationValue());
                
            CreateMap<Event, EventProcessModel>().ReverseMap();
            CreateMap<EventDetail, EventDetailModel>().ReverseMap();
            CreateMap<EventDetail, EventDetailOperatorModel>().ReverseMap();
            CreateMap<EventDetail, EventDetailProcessModel>().ReverseMap();
            CreateMap<Event, EventTicketModel>().ReverseMap();

            CreateMap<Ticket, TicketModel>().
                ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.EventDetail.Event));

            CreateMap<SponsorEvent, SponsorEventModel>();
            //.ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event))
            //.ForMember(dest => dest.Transaction, opt => opt.MapFrom(src => src.Transaction));
            CreateMap<SponsorManagement, SponsorManagementModel>();
                //.ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event));

            CreateMap<Feedback, FeedBackModel>().ReverseMap();

            CreateMap<DAO.EntityModels.Task,TaskModel>().ReverseMap();
            CreateMap<DAO.EntityModels.Task,TaskStatusModel>().ReverseMap();

            CreateMap<EventStall,EventStallModel>().
                ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.EventDetail.Event)); ;

            CreateMap<EventAssignee,EventAssigneeModel>().ReverseMap();

            CreateMap<SponsorEvent, SponsorEventModel>().ReverseMap();
            CreateMap<SponsorManagement, SponsorManagementModel>().ReverseMap();
        }
    }
}
