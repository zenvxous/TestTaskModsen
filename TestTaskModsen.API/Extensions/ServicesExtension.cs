using TestTaskModsen.Application.Services;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Mappers;
using TestTaskModsen.Persistence.Repositories;

namespace TestTaskModsen.API.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IMapper<EventEntity, Event>, EventMapper>();
        services.AddScoped<IMapper<UserEntity, User>, UserMapper>();
        services.AddScoped<IMapper<RegistrationEntity, Registration>, RegistrationMapper>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IEventService, EventService>();
        
        return services;
    }
}