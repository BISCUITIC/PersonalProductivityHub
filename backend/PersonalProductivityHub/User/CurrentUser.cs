using Application.Contracts;
using System.Security.Claims;

namespace PersonalProductivityHub.User;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal? _claimsPrincipal;
    private Guid? _userId;
    public Guid Id => _userId ??= ParseId();

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _claimsPrincipal = _httpContextAccessor.HttpContext?.User;
    }

    private Guid ParseId()
    {
        string? id = _claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        bool success = Guid.TryParse(id, out Guid userId);

        if (success)
        {
            return userId;
        }

        return Guid.Empty;
    }
}
