using System.ComponentModel.DataAnnotations;

public class BusinessCreateDto 
{
    // id property should be here according to api contract, but that does not make sense

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