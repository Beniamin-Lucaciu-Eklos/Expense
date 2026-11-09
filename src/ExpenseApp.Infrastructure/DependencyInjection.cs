using System;
using ExpenseApp.Application.Abstractions;
using ExpenseApp.Application.Services;
using ExpenseApp.Domain.Data.Models;
using ExpenseApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ExpenseDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DB"));
        });

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITransactionUnitOfWork, TransactionUnitOfWork>();


        return services;
    }

}
