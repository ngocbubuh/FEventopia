using FEventopia.Controllers.Middlewares;
using FEventopia.Controllers.ExceptionHandlers;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Repositories.Repositories;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Services;
using System.Diagnostics;
using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DAO;
using Microsoft.EntityFrameworkCore;
using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels;
using Microsoft.AspNetCore.Identity;
using AspNetCoreRateLimit;

namespace FEventopia.Controllers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            //Add Scope
            services.AddScoped(typeof(IGenericDAO<>), typeof(GenericDAO<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            services.AddScoped<IFeedBackService, FeedBackService>();
            services.AddScoped<IFeedBackRepository, FeedBackRepository>();

            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddScoped<IEventStallService, EventStallService>();
            services.AddScoped<IEventStallRepository, EventStallRepository>();

            services.AddScoped<IEventAssigneeRepository, EventAssigneeRepository>();
            services.AddScoped<IEventAssigneeService, EventAssigneeService>();

            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAuthenService, AuthenService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventDetailRepository, EventDetailRepository>();
            services.AddScoped<IEventDetailService, EventDetailService>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ISponsorManagementRepository, SponsorManagementRepository>();
            services.AddScoped<ISponsorManagementService, SponsorManagementService>();
            services.AddScoped<ISponsorEventRepository, SponsorEventRepository>();
            services.AddScoped<ISponsorEventService, SponsorEventService>();
            services.AddScoped<IAnalysisService, AnalysisService>();

            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IEventDAO, EventDAO>();
            services.AddScoped<IEventDetailDAO, EventDetailDAO>();
            services.AddScoped<ITicketDAO, TicketDAO>();
            services.AddScoped<ISponsorEventDAO, SponsorEventDAO>();
            services.AddScoped<ISponsorManagementDAO, SponsorManagementDAO>();
            services.AddScoped<IEventStallDAO, EventStallDAO>();
            services.AddScoped<IEventAssigneeDAO, EventAssigneeDAO>();

            //Add Exception Handler
            services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            //Add Middleware
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            services.AddProblemDetails();
            services.AddDbContext<FEventopiaDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING"));
            });
            services.AddIdentity<Account, IdentityRole>()
                    .AddEntityFrameworkStores<FEventopiaDbContext>().AddDefaultTokenProviders();

            //Add Brute Force setting
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            
            return services;
        }
    }
}
