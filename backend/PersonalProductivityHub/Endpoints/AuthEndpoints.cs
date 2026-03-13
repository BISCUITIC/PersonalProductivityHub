using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using PersonalProductivityHub.Contracts.Auth;

namespace PersonalProductivityHub.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/logout", Logout);
    }
    private async static Task<IResult> Register(RegisterRequest request,
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
            return Results.BadRequest(register_result.Errors.Select(errors => errors.Description));
        }

        AuthResponse response = new AuthResponse(newUser.UserName, newUser.Email, newUser.CreatedAt);

        return Results.Ok(response);      
    }

    private async static Task<IResult> Login(LoginRequest request, 
                                             SignInManager<ApplicationUser> signInManager)
    {
        SignInResult result = await signInManager.PasswordSignInAsync(userName: request.UserName,
                                                                      password: request.Password,
                                                                      isPersistent: true,
                                                                      lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }

        ApplicationUser? user = await signInManager.UserManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        AuthResponse response = new AuthResponse(user.UserName ?? "", user.Email ?? "", user.CreatedAt);

        return Results.Ok(response);    
    }

    private async static Task<IResult> Logout(SignInManager<ApplicationUser> signInManager) 
    {        
        await signInManager.SignOutAsync();

        return Results.Ok("Logged out");     
    }
}
