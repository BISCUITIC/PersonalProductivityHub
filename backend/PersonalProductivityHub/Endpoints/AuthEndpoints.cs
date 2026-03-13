using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using PersonalProductivityHub.Contracts;

namespace PersonalProductivityHub.Endpoints;

public static class AuthEndpoints
{
    public static void MapRegistrationEndpoints(this WebApplication application)
    {
        application.MapPost("/register", async (RegisterRequest request,
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
    }

    public static void MapLoginEndpoints(this WebApplication application)
    {
        application.MapPost("/login", async (LoginRequest request, SignInManager<ApplicationUser> signInManager) =>
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
    }

    public static void MapLogoutEndpoints(this WebApplication application) 
    {
        application.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();

            return Results.Ok("Logged out");
        });
    }
}
