using System; 

namespace PSPBackend.Model
{
    public class OrderModel
    {
        public int id {get; set; }
        public int? business_id {get; set; }
        public int? employee_id  {get; set; }
        public decimal order_discount_percentage {get; set; }
        public decimal total_amount {get; set; }
        public decimal tax_amount  {get; set; }
        public decimal total_discount_amount {get; set; }
        public int? order_status {get; set; }
        public DateTime created_at {get; set; }
        public DateTime closed_at {get; set; }

        public OrderModel()
        {
            id = 1;
            business_id = null;
            employee_id = null;
            order_status = null;
            created_at = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Order ID: {id}, " +
                   $"Business ID: {business_id}, " +
                   $"Employee ID: {employee_id}, " +
                   $"Total Amount: {total_amount}, " +
                   $"Tax Amount: {tax_amount}, " +
                   $"Discount: {order_discount_percentage}%, " +
                   $"Total Discount Amount: {total_discount_amount}, " +
                   $"Order Status: {order_status}, " +
                   $"Created At: {created_at}, " +
                   $"Closed At: {closed_at}";
        }
    }
    
}
