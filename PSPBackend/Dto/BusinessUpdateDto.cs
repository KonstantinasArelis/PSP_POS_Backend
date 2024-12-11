public class BusinessUpdateDto 
{
    // TO-DO documentation says there should be id field in here, but it is already passed by route
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