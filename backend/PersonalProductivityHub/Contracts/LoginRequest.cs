using System.ComponentModel.DataAnnotations;

namespace PersonalProductivityHub.Contracts;

public record LoginRequest(
    [Required, StringLength(128)]
    string UserName, 
    [Required, MinLength(6)]
    string Password
);
