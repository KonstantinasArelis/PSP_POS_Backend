public class GiftCardCreateDto
    {
        public decimal? original_amount { get; set; }
        public DateTime? valid_from { get; set; }
        public DateTime? valid_until { get; set; }
    }