using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Infrastructure.Persistence;
using BloggingPlatform.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace BloggingPlatform.Infrastructure.DependencyInjection;
public static class InfrastructureServiceRegistration{public static IServiceCollection AddInfrastructure(this IServiceCollection s,IConfiguration c){s.AddDbContext<AppDbContext>(o=>o.UseSqlServer(c.GetConnectionString("DefaultConnection")));s.AddScoped<IJwtTokenService,JwtTokenService>();return s;}}
