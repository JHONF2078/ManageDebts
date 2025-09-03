using System.Text;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Infrastructure.Auth;
using ManageDebts.Infrastructure.Auth.Options;
using ManageDebts.Infrastructure.Identity;
using ManageDebts.Infrastructure.Persistence;
using ManageDebts.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ManageDebts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // DbContext (PostgreSQL) usando variables de entorno
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "manage_debs_db";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "admin_user";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "jhon.123";
            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(connectionString));

            // Identity
            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 8;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager();

            // Options
            services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
            services.Configure<RefreshTokenOptions>(config.GetSection(RefreshTokenOptions.SectionName));

            // JWT Auth
            var jwt = config.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                    //ValidateLifetime = true,
                };
            });

            // Redis Cache
            var redisConn = config.GetSection("Redis:ConnectionString").Value;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
            });
            services.AddScoped<ICacheService, RedisCacheService>();

            // Servicios
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ManageDebts.Domain.Repositories.IDebtRepository, Repository.DebtRepository>();
            services.AddScoped<IUserService, Services.UserService>();

            return services;
        }
    }
}
