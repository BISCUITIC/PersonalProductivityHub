using Domain.Contracts;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PersonalProductivityHub.Contracts.Auth;
using PersonalProductivityHub.Endpoints;
using PersonalProductivityHub.Extensions;

namespace PersonalProductivityHub;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

        builder.AddDatabase();
        builder.AddIdentity();
        builder.AddSwagger();
        builder.Services.AddProblemDetails();

        builder.Services.AddAuthorization();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("frontend", builder =>
            {
                builder.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseCors("frontend");

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Разрешаем отправку cookie при тестировании
                options.ConfigObject.AdditionalItems["withCredentials"] = true;
            });
        }
        else
        {
            app.UseExceptionHandler();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();
        app.MapProjectEndpoints();

        app.MapGet("/profile", [Authorize] async (HttpContext http, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.GetUserAsync(http.User);

            if (user is null)
                return Results.Unauthorized();

            var response = new AuthResponse(user.UserName ?? "", user.Email ?? "", user.CreatedAt);

            return Results.Ok(response);
        });

        app.Run();
    }
}
