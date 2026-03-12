using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
