namespace PSPBackend.Model;
public class OrderArgumentModel
{
    public int? PageNr { get; set; }
    public int? Limit { get; set; }
    public int? EmployeeId { get; set; }
    public decimal? MinTotalAmount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public decimal? MinTipAmount { get; set; }
    public decimal? MaxTipAmount { get; set; }
    public decimal? MinTaxAmount { get; set; }
    public decimal? MaxTaxAmount { get; set; }
    public decimal? MinDiscountAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal? MinOrderDiscountPercentage { get; set; }
    public decimal? MaxOrderDiscountPercentage { get; set; }
    public string? CreatedBefore { get; set; }
    public string? CreatedAfter { get; set; }
    public string? ClosedBefore { get; set; }
    public string? ClosedAfter { get; set; }
    public string? OrderStatus { get; set; }
}