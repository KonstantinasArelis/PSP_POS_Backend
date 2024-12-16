namespace PSPBackend.DTO
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public string? Role { get; set; }
        public int? BusinessId { get; set; }
    }
}
