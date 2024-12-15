namespace PSPBackend.Dto;

public class ProductGetDto
{
    public int? page_nr { get; set; }
    public int? limit { get; set; }
    public int? business_id { get; set; }
    public decimal? min_price { get; set; }
    public decimal? max_price { get; set; }
    public string? product_type { get; set; } // Enum values for SERVICE, ITEM, etc., can be mapped
    public bool? is_for_sale { get; set; }
    public int? tax_id { get; set; }
    public int? category_id { get; set; }
    public bool? can_discount_be_applied { get; set; }
    public string? variations { get; set; }
}

