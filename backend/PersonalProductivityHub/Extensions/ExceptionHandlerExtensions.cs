using Microsoft.AspNetCore.Mvc;

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

                var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

                ProblemDetails problem;

                if (exception is ArgumentException argEx)
                {
                    problem = new ProblemDetails
                    {
                        Title = "Invalid input",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = argEx.Message
                    };
                }
                else
                {
                    problem = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = exception?.Message
                    };
                }

                context.Response.StatusCode = problem.Status ?? 500;

                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}
