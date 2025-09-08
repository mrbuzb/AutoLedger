namespace AutoLedger.Application.Dtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public UserGetDto User { get; set; }
    public string TokenType { get; set; }
    public int Expires { get; set; }
}
