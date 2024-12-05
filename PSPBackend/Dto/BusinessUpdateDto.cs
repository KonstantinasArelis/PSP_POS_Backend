public class BusinessUpdateDto 
{
    // TO-DO documentation says there should be id field in here, but it is already passed by route
    public string? name { get; set; }
    public string? address { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public string? currency { get; set; }
}