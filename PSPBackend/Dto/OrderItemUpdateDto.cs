namespace PSPBackend.Utility;

public class OrderItemUpdateDto
{
    public int quantity { get; set; }
    public IEnumerable<Variation>? variations {get; set;}
}