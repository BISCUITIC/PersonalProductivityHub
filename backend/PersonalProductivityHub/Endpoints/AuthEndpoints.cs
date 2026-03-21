using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using PersonalProductivityHub.Contracts.Auth;
using PersonalProductivityHub.Mappings;
using System.Security.Claims;

namespace PersonalProductivityHub.Endpoints;

/// <summary>
/// TODO : Add application layer for services. Use pattern Result
/// </summary>

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/logout", Logout).RequireAuthorization();
        group.MapGet("/me", Me).RequireAuthorization();
    }

    private static async Task<IResult> Register(RegisterRequest request,
                                                UserManager<ApplicationUser> userManager)
    {
        ApplicationUser newUser = new ApplicationUser()
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        IdentityResult register_result = await userManager.CreateAsync(newUser, request.Password);

        if (!register_result.Succeeded)
        {            
            return Results.ValidationProblem(
                   register_result.Errors
                                  .GroupBy(error => error.Code)
                                  .ToDictionary(group => group.Key,
                                                group => group.Select(error => error.Description).ToArray()));
        }

        AuthResponse response = newUser.ToAuthResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> Login(LoginRequest request,
                                             SignInManager<ApplicationUser> signInManager)
    {
        ApplicationUser? user = await signInManager.UserManager
                                                   .FindByNameAsync(request.UserName);

        if (user is null)
        {
            return InvalidUsernameOrPassword();
        }

        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return InvalidUsernameOrPassword();
        }

        await signInManager.SignInAsync(user, isPersistent: true);
        
        AuthResponse response = user.ToAuthResponse();

        return Results.Ok(response);
    }

    private static async Task<IResult> Logout(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();

        return Results.Ok();
    }

    private static async Task<IResult> Me(ClaimsPrincipal claimsPrincipal, UserManager<ApplicationUser> userManager)
    {
        ApplicationUser? user = await userManager.GetUserAsync(claimsPrincipal);

        if (user is null)
        {
            return InvalidUsernameOrPassword();
        }

        AuthResponse response = user.ToAuthResponse();

        return Results.Ok(response);
    }

    private static IResult InvalidUsernameOrPassword()
    {
        return Results.Problem(title: "Invalid username or password",
                               statusCode: StatusCodes.Status401Unauthorized);
    }

}
