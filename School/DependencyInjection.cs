using Asp.Versioning;
using FluentValidation;
using School.Api.Filter;
using School.Api.Mapping;

namespace School.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddApiVersioning();
            services.AddMappers();
            services.AddScalar();
            services.AddSwagger();
            services.AddValidation();
            services.AddControllerAndValidaion();
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
            services.AddAutoMapper(autoMapper =>
            { }, typeof(MappingProfile).Assembly);
        }
        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
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
    }
}
