using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts;

public record RegisterRequest(
    [Required, StringLength(128)]
    string UserName, 
    [Required, EmailAddress]
    string Email, 
    [Required, MinLength(6)]
    string Password
);
