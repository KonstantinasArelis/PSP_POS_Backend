namespace PSPBackend.Model
{
    public class RefundModel
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int OrderItemId { get; set; }
        public bool ReturnedToInventory { get; set; }
        public int RefundedQuantity { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
