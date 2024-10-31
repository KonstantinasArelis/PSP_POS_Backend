namespace PSPBackend.Model
{
    public class DiscountModel
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int ProductId { get; set; }
        public int DiscountType { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
    }
}
