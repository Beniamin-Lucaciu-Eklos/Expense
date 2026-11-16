using System;
using ExpenseApp.Domain.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var currentAssembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(currentAssembly);

            //configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

            //configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(currentAssembly);

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}
