namespace PSPBackend.Utility;

public class OrderItemUpdate
{
    public int quantity { get; set; }
    public IEnumerable<Variation> variations {get; set;}
}