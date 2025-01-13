using System.ComponentModel.DataAnnotations;

namespace TestTaskModsen.API.Contracts.User;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);