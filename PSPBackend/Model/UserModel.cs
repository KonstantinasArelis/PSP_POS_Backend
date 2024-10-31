namespace PSPBackend.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public string UserName { get; set; }
        public string UserUsername { get; set; }
        public int UserRole { get; set; }
        public string PasswordHash { get; set; }
        public decimal TipsAmount { get; set; }
        public DateTime LastWithdrawnTimestamp { get; set; }
        public int AccountStatus { get; set; }
    }
}
