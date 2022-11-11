using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REDChallenge.Application.Middleware;
using REDChallenge.Application.Models;
using REDChallenge.Application.ServiceInterface;
using REDChallenge.Application.Services;
using REDChallenge.Application.Validators.Order;
using REDChallenge.Domain.Entities;
using REDChallenge.Domain.Repository;
using REDChallenge.Persistance;
using REDChallenge.Persistance.Repository;

namespace REDChallenge;
public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();
        services.AddFluentValidationClientsideAdapters();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        var appSettingsSection = Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = "my-issuer",
                ValidAudience = "Test",
            };
        });
        services.AddAuthorization();

        services.AddSwaggerGen(swag =>
        {
            swag.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Order RED Challenge",
                Description = "This is a code challenge",
                Version = "v1",
            });
            swag.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authentication token"
            });
            swag.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });

        services.AddDbContext<REDChallengeContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("REDChallengeContext"))
        );

        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient(typeof(IRepository<Customer, Guid>), typeof(GenericRepository<Customer, Guid>));

        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var appSettingsSection = Configuration.GetSection("AppSettings");
        var appSettings = appSettingsSection.Get<AppSettings>();

        if (appSettings.EnableSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}

