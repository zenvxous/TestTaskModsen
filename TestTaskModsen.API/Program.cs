using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using TestTaskModsen.API.Extensions;
using TestTaskModsen.Application.Authentication;
using TestTaskModsen.Application.Services;
using TestTaskModsen.Core.Interfaces.Authentication;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Mappers;
using TestTaskModsen.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IMapper<EventEntity, Event>, EventMapper>();
builder.Services.AddScoped<IMapper<UserEntity, User>, UserMapper>();
builder.Services.AddScoped<IMapper<RegistrationEntity, Registration>, RegistrationMapper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IEventService, EventService>();

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.Run();