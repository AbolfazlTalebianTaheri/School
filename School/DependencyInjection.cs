using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using School.Api.Filter;
using School.Api.Mapping;
using School.Api.Mapping.Attendance;
using School.Api.Mapping.Student;
using School.Api.Mapping.User;
using School.Application.InterFaces;
using School.Infrastructure.Authentication;
using System.Text;
using System.Threading.RateLimiting;

namespace School.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning();
            services.AddMappers();
            services.AddScalar();
            services.AddSwagger();
            services.AddValidation();
            services.AddControllerAndValidaion();
            services.AddLifeTimeJwt();
            services.AddJwtSetting(configuration);
            services.AddFrontendCors(configuration);
            services.AddLoginRateLimiter();
            return services;
        }
        private static void AddApiVersioning(this IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    options.ReportApiVersions = true;
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
        private static void AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(
                cfg => { },
                typeof(MappingProfile).Assembly,
                typeof(MapingAttendanceDelay).Assembly,
                typeof(MapingAttendanceLog).Assembly,
                typeof(MappingLesson).Assembly,
                typeof(MappingStudent).Assembly,
                typeof(MappingUser).Assembly
            );
        }
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "School API",
                    Version = "v1",
                    Description = "School Management API"
                });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Access Token خود را وارد کنید."
                    });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(
                            "Bearer", document)] = []
                    });
            });
        }
        private static void AddScalar(this IServiceCollection services)
        {

        }
        private static void AddValidation(this IServiceCollection services)
        {
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        }
        private static void AddControllerAndValidaion(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                //options.Filters.Add<ApiResultFilter>();
            });
        }
        private static void AddLifeTimeJwt(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, JwtTokenService>();
        }
        private static void AddLoginRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy("login", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));
            });
        }
        private static void AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
            services.AddCors(options =>
            {
                if (origins.Length > 0)
                    options.AddDefaultPolicy(policy => policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }
        private static void AddJwtSetting(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSettings>() ?? throw new InvalidOperationException("HwtSetting configuration is missing");
            if (string.IsNullOrWhiteSpace(jwtSetting.SecretKey))
                throw new InvalidOperationException("Jwt SecretKey is not configured");
            services.Configure<JwtSettings>(configuration.GetSection("JwtSetting"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSetting.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSetting.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    jwtSetting.SecretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            });
            services.AddAuthorization();
        }
    }
}
