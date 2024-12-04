namespace PSPBackend.Model
{
    public class GiftCardModel
    {
        public int id { get; set; }
        public decimal original_amount { get; set; }
        public decimal amount_left { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime valid_until { get; set; }
        public string code_hash { get; set; }
    }
}
