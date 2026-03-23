using Infrastructure.Identity;
using PersonalProductivityHub.Contracts.Auth;

namespace PersonalProductivityHub.Mappings;

public static class ApplicationUserExtensions
{
    public static AuthResponse ToAuthResponse(this ApplicationUser user)
    {
        return new AuthResponse(user.UserName ?? "", user.Email ?? "", user.CreatedAt);
    }
}
