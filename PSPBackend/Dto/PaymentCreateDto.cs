public class PaymentCreateDto {
    public int? id {get; set;} 
    public int? order_id { get; set; }
    public decimal? total_amount { get; set; }
    public decimal? order_amount { get; set; }
    public decimal? tip_amount { get; set; }
    public int? payment_method { get; set; }
    public DateTime? created_at { get; set; }
    public int? status { get; set; }
    public int? gift_card_id { get; set; }
}