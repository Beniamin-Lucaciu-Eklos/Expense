using System.Text.Json.Serialization;
using ExpenseApp.Api.Authorization;
using ExpenseApp.Api.Constants;
using ExpenseApp.Application;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Infrastructure;
using ExpenseApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseApp.Domain.Data.Models;
using Serilog;

namespace ExpenseApp.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builderArgs = args
            .Where(argument =>
                !string.Equals(argument, "--seed-sample-data", StringComparison.OrdinalIgnoreCase) &&
                !argument.StartsWith("--seed-count=", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var builder = WebApplication.CreateBuilder(builderArgs);

        // Add services to the container.

        builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

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

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthenticationFeatures(builder.Configuration);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddScoped<IUserClaimPrincipal, UserClaimPrincipal>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (args.Contains("--seed-sample-data", StringComparer.OrdinalIgnoreCase))
        {
            if (!app.Environment.IsDevelopment())
            {
                throw new InvalidOperationException(
                    "Sample data can only be seeded when the API is running in Development.");
            }

            var recordCount = ReadSeedRecordCount(args);
            await using var scope = app.Services.CreateAsyncScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DevelopmentSampleDataSeeder>();
            await seeder.SeedAsync(recordCount);
            return;
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(CorsConstants.DefaultPolicy);

        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        app.UseCustomExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static int ReadSeedRecordCount(string[] args)
    {
        const string optionPrefix = "--seed-count=";
        var countOption = args.FirstOrDefault(argument =>
            argument.StartsWith(optionPrefix, StringComparison.OrdinalIgnoreCase));

        if (countOption is null)
        {
            return 1_000;
        }

        if (!int.TryParse(countOption[optionPrefix.Length..], out var recordCount) ||
            recordCount is < 1 or > 1_000_000)
        {
            throw new ArgumentException(
                "The --seed-count value must be between 1 and 1,000,000.");
        }

        return recordCount;
    }
}
