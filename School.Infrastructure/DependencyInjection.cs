using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.InterFaces;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Infrastructure.Authentication;
using School.Infrastructure.Persistencies.EfCore.PostgerSql;
using School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository;
using School.Infrastructure.Persistencies.Mongo;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace School.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            //services.AddMongo(configuration);
            services.AddPasswordHasherLifeTime();
        }
        private static void AddPasswordHasherLifeTime(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IPasswordService, PasswordService>();
        }
        private static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSetting>(configuration.GetSection(MongoSetting.SectionName));
            MongoMappingConfig.Register();
            services.AddSingleton<MongoContext>();
            services.AddScoped<IExceptionLogRepository, IExceptionLogRepository>();
        }
    }
}
