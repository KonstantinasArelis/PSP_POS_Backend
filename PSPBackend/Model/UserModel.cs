using Microsoft.AspNetCore.Identity;

namespace PSPBackend.Model
{
    public class UserModel : IdentityUser
    {
        public int? BusinessId { get; set; }
        public string? FullName { get; set; }
        public decimal? TipsAmount { get; set; }
        public DateTime? LastWithdrawnTimestamp { get; set; }
        public int? AccountStatus { get; set; }
    }
}
