using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Validation;
using Scalar.AspNetCore;
using School.Api;
using School.Api.Mapping;
using School.Api.Middleware;
using School.Application;
using School.Infrastructure;
using School.Infrastructure.Authentication;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAplication();
builder.Services.AddApi(builder.Configuration);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
