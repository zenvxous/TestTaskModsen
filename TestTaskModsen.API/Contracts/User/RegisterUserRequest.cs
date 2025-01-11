using System.ComponentModel.DataAnnotations;

namespace TestTaskModsen.API.Contracts.User;

public record RegisterUserRequest(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string Password);