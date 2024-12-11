public class BusinessGetDto 
{
    [Range(0, int.MaxValue, ErrorMessage = "Page number must be a non-negative integer.")]
    public int page_nr { get; set; } = 0;

    [Range(0, 100, ErrorMessage = "Page limit must be between 0 and 100")]
    public int limit { get; set; } = 20;
    [BusinessName]
    public string? name { get; set; }

    [BusinessAddress]
    public string? address { get; set; }

    [phone]
    public string? phone { get; set; }

    [email]
    public string? email { get; set; }

    [currency]
    public string? currency { get; set; }
}