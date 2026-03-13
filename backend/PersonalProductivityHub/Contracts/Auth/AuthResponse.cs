using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts.Auth;

public sealed record AuthResponse(
    [Required]
    string UserName, 
    [Required, EmailAddress]
    string Email, 
    [Required]
    DateTime CreatedAt
);
