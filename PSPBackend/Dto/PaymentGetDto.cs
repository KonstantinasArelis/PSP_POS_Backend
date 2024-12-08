using System.Text.Json.Serialization;

public class PaymentGetDto {
    public int page_nr {get; set;}
    public int limit {get; set;} 
    public int? order_id { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public paymentMethodEnum? payment_method { get; set; }
    public DateTime? created_before { get; set; }
    public DateTime? created_after { get; set; }
    public paymentStatusEnum? status { get; set; }
}