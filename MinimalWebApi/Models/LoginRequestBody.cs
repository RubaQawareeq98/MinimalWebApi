namespace MinimalWebApi.Models;

public class LoginRequestBody
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
