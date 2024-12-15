namespace PSPBackend.Dto;
public class OrderGetDto
{
    public int? page_nr { get; set; }
    public int? limit { get; set; }
    public int? employee_id { get; set; }
    public decimal? min_total_amount { get; set; }
    public decimal? max_total_amount { get; set; }
    public decimal? min_tax_amount { get; set; }
    public decimal? max_tax_amount { get; set; }
    public decimal? min_discount_amount { get; set; }
    public decimal? max_discount_amount { get; set; }
    public decimal? min_order_discount_percentage { get; set; }
    public decimal? max_order_discount_percentage { get; set; }
    public string? created_before { get; set; }
    public string? created_after { get; set; }
    public string? closed_before { get; set; }
    public string? closed_after { get; set; }
    public string? order_status { get; set; }
}