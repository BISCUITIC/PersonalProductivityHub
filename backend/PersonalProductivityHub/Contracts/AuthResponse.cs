using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts;

public record AuthResponse(
    [Required]
    string UserName, 
    [Required, EmailAddress]
    string Email, 
    [Required]
    DateTime CreatedAt
);
