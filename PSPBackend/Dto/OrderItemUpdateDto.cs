namespace PSPBackend.Dto;

public class OrderItemUpdateDto
{
    public int quantity { get; set; }
    public IEnumerable<VariationDto>? variations {get; set;}
}