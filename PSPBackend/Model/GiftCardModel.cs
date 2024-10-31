namespace PSPBackend.Model
{
    public class GiftCardModel
    {
        public int Id { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal AmountLeft { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string CodeHash { get; set; }
    }
}
