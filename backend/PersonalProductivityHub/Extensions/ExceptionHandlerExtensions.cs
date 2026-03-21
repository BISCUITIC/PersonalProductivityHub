using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PersonalProductivityHub.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void UseConfiguredExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionApp =>
        {
            exceptionApp.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                Exception? exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                ProblemDetails problem = new ProblemDetails()
                {
                    Type = "https://example.com/internal-server-error",
                    Title = "Internal Server Error",
                    Status = 500,
                    Detail = app.Environment.IsDevelopment() ? exception?.Message : "An unexpected error occurred",
                    Extensions = new Dictionary<string, object?>() { { "traceId", context.TraceIdentifier } }
                };

                context.Response.StatusCode = problem.Status ?? 500;
                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}
