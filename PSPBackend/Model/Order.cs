using System; 

namespace PSPBackend.Model
{
    public class Order
    {
        public int id {get; set; }
        public int business_id {get; set; }
        public int employee_id  {get; set; }
        public decimal order_discount_percentage {get; set; }
        public decimal total_amount {get; set; }
        public decimal tax_amount  {get; set; }
        public decimal total_discount_amount {get; set; }
        public int order_status {get; set; }
        public DateTime created_at {get; set; }
        public DateTime closed_at {get; set; }
    }
}
