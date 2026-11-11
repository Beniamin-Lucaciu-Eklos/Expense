using System.Text.Json.Serialization;
using ExpenseApp.Api.Constants;
using ExpenseApp.Application;
using ExpenseApp.Infrastructure;
using ExpenseApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
                .AddJsonOptions(config =>
                {
                    config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsConstants.DefaultPolicy,
                opt =>
                {
                    opt.AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowAnyOrigin();
                });
        });

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(CorsConstants.DefaultPolicy);

        app.UseHttpsRedirection();

        app.UseCustomExceptionHandler();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
