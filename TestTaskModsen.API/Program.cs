using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Mappers;
using TestTaskModsen.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IMapper<EventEntity, Event>, EventMapper>();
builder.Services.AddSingleton<IMapper<UserEntity, User>, UserMapper>();
builder.Services.AddSingleton<IMapper<RegistrationEntity, Registration>, RegistrationMapper>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddSingleton<IRegistrationRepository, RegistrationRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();