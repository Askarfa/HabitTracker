using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Repositories;
using HabitTracker.Services;
using HabitTracker.Endpoints;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using HabitTracker.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HabitTracker API",
        Version = "v1",
        Description = "API для отслеживания привычек и целей"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT авторизация. Введите: Bearer {ваш токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IHabitLogRepository, HabitLogRepository>();

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IHabitLogService, HabitLogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(cors => cors
    .WithOrigins("https://localhost:7186")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHabitEndpoints();
app.MapGoalEndpoints();
app.MapHabitLogEndpoints();
app.MapAuthEndpoints();

app.Run();