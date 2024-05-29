using FEventopia.Controllers.Middlewares;
using FEventopia.Controllers.ExceptionHandlers;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Repositories.Repositories;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Services;
using System.Diagnostics;
using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DAO;

namespace FEventopia.Controllers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            //Add Scope
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped(typeof(IGenericDAO<>), typeof(GenericDAO<>));

            //Add Exception Handler
            //services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            
            //Add Middleware
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            services.AddProblemDetails();

            return services;
        }
    }
}
