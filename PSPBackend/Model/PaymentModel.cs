namespace PSPBackend.Model
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal TipAmount { get; set; }
        public int PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PaymentStatus { get; set; }
        public int GiftCardId { get; set; }
    }
}
