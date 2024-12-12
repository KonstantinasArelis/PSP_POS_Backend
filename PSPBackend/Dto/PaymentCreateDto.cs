using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

public class PaymentCreateDto {
    // TO-DO documentation says id is supposed to be here, but that dont make sense
    [Range(1, int.MaxValue, ErrorMessage = "order Id must be a positive integer.")] 
    public int order_id { get; set; } // TO-DO is it safe to pass info like this

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Total amount must be a positive decimal.")] 
    public decimal total_amount { get; set; }

    // no idea what this field is, so no validation for you
    public decimal? order_amount { get; set; } // TO-DO what is this field

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Tip amount must be a positive decimal.")] 
    public decimal tip_amount { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public paymentMethodEnum payment_method { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Gift card Id must be a positive integer or null")] 
    public int? gift_card_id { get; set; }
}