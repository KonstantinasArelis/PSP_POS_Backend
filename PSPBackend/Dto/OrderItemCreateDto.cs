namespace PSPBackend.Dto;
public class OrderItemCreateDto
{
    public int order_id { get; set; }
    public int? product_id { get; set; }
    public int? quantity { get; set; }
    public IEnumerable<VariationDto> variations { get; set; }
}