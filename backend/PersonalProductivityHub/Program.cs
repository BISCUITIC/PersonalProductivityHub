using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PersonalProductivityHub.Extensions;
using PersonalProductivityHub.Contracts;

namespace PersonalProductivityHub;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddDatabase();
        builder.AddIdentity();
        builder.AddSwagger();

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
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapPost("/register", async (RegisterRequest request,
                                        UserManager<ApplicationUser> userManager) =>
        {
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = request.UserName,
                Email = request.Email,
            };

            var register_result = await userManager.CreateAsync(newUser, request.Password);
            
            if (!register_result.Succeeded)
            {
                return Results.BadRequest(register_result.Errors.Select(errors => errors.Description));
            }

            var response = new AuthResponse(newUser.UserName, newUser.Email, newUser.CreatedAt);

            return Results.Ok(response);
        });

        app.MapPost("/login", async (LoginRequest request, SignInManager<ApplicationUser> signInManager) =>
        {
            var result = await signInManager.PasswordSignInAsync(userName: request.UserName,
                                                                 password: request.Password,
                                                                 isPersistent: true,
                                                                 lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }
            
            var user = await signInManager.UserManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            var response = new AuthResponse(user.UserName ?? "", user.Email ?? "", user.CreatedAt);

            return Results.Ok(response);            
        });

        app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();

            return Results.Ok("Logged out");
        });

        app.MapGet("/profile", [Authorize] async (HttpContext http, UserManager < ApplicationUser> userManager) =>
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
