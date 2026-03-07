
using Microsoft.EntityFrameworkCore;
using RefundEligibilitySystem.Application.Services;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Domain.IServices;
using RefundEligibilitySystem.Infrastructure.Data;
using RefundEligibilitySystem.Infrastructure.Repositories;
using Scalar.AspNetCore;

namespace RefundEligibilitySystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<RefundDbContext>(options =>
                options.UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information));

            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<ICitizenRepository, CitizenRepository>();
            builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<ICitizenService, CitizenService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                    policy.WithOrigins(
                        "http://localhost:3000",
                        "https://localhost:3000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseCors("AllowReactApp");

            await app.Services.InitializeDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
