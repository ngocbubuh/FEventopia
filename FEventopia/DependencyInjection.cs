using FEventopia.Controllers.Middlewares;
using FEventopia.Controllers.ExceptionHandlers;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Repositories.Repositories;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Services;
using System.Diagnostics;
using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DAO;
using FEventopia.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

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

            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IEventDAO, EventDAO>();
            services.AddScoped<IEventDetailDAO, EventDetailDAO>();
            services.AddScoped<ITicketDAO, TicketDAO>();
            services.AddScoped<ISponsorEventDAO, SponsorEventDAO>();
            services.AddScoped<ISponsorManagementDAO, SponsorManagementDAO>();
            services.AddScoped<IEventStallDAO, EventStallDAO>();

            //Add Exception Handler
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            
            //Add Middleware
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            services.AddProblemDetails();

            return services;
        }
    }
}
