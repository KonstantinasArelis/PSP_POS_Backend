using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PSPBackend.Model;

public class OrderItemModel
{
    public int id { get; set; }
    [ForeignKey("Order")]
    public int order_id { get; set; }
    public int? product_id { get; set; }
    public int? reservation_id { get; set; }
    public int? quantity { get; set; }
    public string? variations { get; set; }
    public string? product_name { get; set; }
    public decimal? product_price { get; set; }
    public int? tax_id { get; set; }
    public decimal? variation_price { get; set; }
    public decimal? item_discount_amount { get; set; }
    [JsonIgnore]
    public OrderModel Order { get; set; }

    public OrderItemModel()
    {
        id = 0;
    }
}