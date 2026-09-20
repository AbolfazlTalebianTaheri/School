using Microsoft.Extensions.DependencyInjection;
using School.Application.Mapper;
using School.Application.Repository;
using School.Application.Service;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace School.Application
{
    public static class DependencyInjection
    {
        public static void AddAplication(this IServiceCollection services)
        {
            services.AddAutoMapper(autoMapper=>
            { },typeof(MappingProfile).Assembly);
            services.AddScoped<StudentService>();
            services.AddScoped<UserService>();
            services.AddScoped<AttendanceService>();
            services.AddScoped<LessonService>();
            services.AddScoped<LoginService>();
            services.AddScoped<UserService>();
        }
    }
}
