public class OrderItemModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public  
 int ReservationId { get; set; }
    public int Quantity { get; set; }
    public string Variations { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int TaxId { get; set; }
    public decimal VariationPrice { get; set; }
    public decimal ItemDiscountAmount { get; set; }
}