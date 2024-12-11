using System.Text.Json.Serialization;

public class PaymentGetDto {
    [Range(0, int.MaxValue, ErrorMessage = "Page number must be a non-negative integer.")]
    public int page_nr {get; set;}

    [Range(0, 100, ErrorMessage = "Page limit must be between 0 and 100")]
    public int limit {get; set;} 

    [Range(1, int.MaxValue, ErrorMessage = "order Id must be a positive integer.")] 
    public int? order_id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public paymentMethodEnum? payment_method { get; set; }

    public DateTime? created_before { get; set; }

    public DateTime? created_after { get; set; }
    
    public paymentStatusEnum? status { get; set; }
}