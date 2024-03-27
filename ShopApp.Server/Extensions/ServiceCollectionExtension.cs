using ShopApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using ShopApp.Infrastrucutre.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopApp.Server.Data.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContext(configuration);
            services.AddControllers();
            services.AddAuthentication(configuration);
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ShopAppDbContext>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ShopAppDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwconfig = configuration.GetSection("JwtConfig");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            })
                .AddJwtBearer(options =>
                {
                    string value = configuration["JwtConfig:Secret"];
                    byte[] key=Encoding.ASCII.GetBytes(value);
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //for development purposes, we are not validating the issuer and audience
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireExpirationTime = false,//for development purposes, we are not validating the expiration time of the token
                        ValidateLifetime = true
                    };
                });
            return services;
        }

    }
}
