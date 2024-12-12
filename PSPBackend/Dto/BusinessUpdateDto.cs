using System.ComponentModel.DataAnnotations;

public class BusinessUpdateDto 
{
    // TO-DO documentation says there should be id field in here, but it is already passed by route
    [BusinessNameAttibute]
    public string? name { get; set; }

    [BusinessAddressAttibute]
    public string? address { get; set; }

    [Phone]
    public string? phone { get; set; }

    [EmailAddress]
    public string? email { get; set; }
    
    [CurrencyAttibute]
    public string? currency { get; set; }
}