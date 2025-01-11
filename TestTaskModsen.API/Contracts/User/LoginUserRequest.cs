using System.ComponentModel.DataAnnotations;

namespace TestTaskModsen.API.Contracts.User;

public record class LoginUserRequest(
    [Required] string Email,
    [Required] string Password);