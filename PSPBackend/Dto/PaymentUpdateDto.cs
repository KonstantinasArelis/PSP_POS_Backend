using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class PaymentUpdateDto {
    // TO-DO there was supposed to be id property here, but thats passed as route paramaters, so idk
    [Range(1, int.MaxValue, ErrorMessage = "order Id must be a positive integer.")] 
    public int order_id { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Total amount must be a positive decimal.")] 
    public decimal total_amount { get; set; }

    // TO-DO no idea what this field is, so no validation for you
    public decimal? order_amount { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Tip amount must be a positive decimal.")] 
    public decimal? tip_amount { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public paymentMethodEnum payment_method { get; set; }

    // TO-DO there is supposed to be a created at field here, but thats not up to the client to provide

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public paymentStatusEnum? status { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Gift card Id must be a positive integer or null")]
    public int? gift_card_id { get; set; }

    public int? business_id { get; set; }
}