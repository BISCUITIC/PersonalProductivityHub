using Application.Contracts;
using Application.Services;
using Domain.Contracts;
using Infrastructure.Data.Repositories;
using PersonalProductivityHub.Endpoints;
using PersonalProductivityHub.Extensions;
using PersonalProductivityHub.User;

namespace PersonalProductivityHub;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();

        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

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

        app.UseConfiguredExceptionHandler();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject.AdditionalItems["withCredentials"] = true;
            });
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();
        app.MapProjectEndpoints();
        app.MapProjectTaskEndpoints();

        app.Run();
    }
}
