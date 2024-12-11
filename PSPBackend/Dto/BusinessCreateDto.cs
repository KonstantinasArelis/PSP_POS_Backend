public class BusinessCreateDto 
{
    // id property should be here according to api contract, but that does not make sense

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