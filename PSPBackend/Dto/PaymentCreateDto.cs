public class PaymentCreateDto {
    // TO-DO documentation says id is supposed to be here, but that dont make sense
    public int order_id { get; set; } // TO-DO is it safe to pass info like this
    public decimal total_amount { get; set; }
    public decimal? order_amount { get; set; } // TO-DO what is this field
    public decimal tip_amount { get; set; }
    public int? payment_method { get; set; }
    public int? gift_card_id { get; set; }
}