using System;
using ExpenseApp.Api.Middleware;

namespace ExpenseApp.Api;

public static class DependencyInjection
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
