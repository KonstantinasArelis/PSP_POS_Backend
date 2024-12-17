namespace PSPBackend.Dto
{
    public class LoginResponseDto
    {
        public string Username { get; set; }
        public string AuthToken { get; set; }
        public string? Role { get; set; }
        public int? BusinessId { get; set; }
    }
}
