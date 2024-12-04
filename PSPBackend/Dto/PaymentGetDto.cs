public class PaymentGetDto {
    public int page_nr {get; set;} 
    public int limit {get; set;} 
    public int? order_id { get; set; }
    public int? payment_method { get; set; }
    public DateTime? created_before { get; set; }
    public DateTime? created_after { get; set; }
    public int? status { get; set; }
}