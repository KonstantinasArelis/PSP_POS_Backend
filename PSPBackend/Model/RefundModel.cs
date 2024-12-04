namespace PSPBackend.Model
{
    public class RefundModel
    {
        public int id { get; set; }
        public int business_id { get; set; }
        public int order_item_id { get; set; }
        public bool returned_to_inventory { get; set; }
        public int refunded_quantity { get; set; }
        public decimal amount { get; set; }
        public string reason { get; set; }
        public DateTime created_at { get; set; }
    }
}
