public class BusinessGetDto 
{
    public int page_nr { get; set; } = 0;
    public int limit { get; set; } = 20;
    public string? name { get; set; }
    public string? address { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public string? currency { get; set; }
}