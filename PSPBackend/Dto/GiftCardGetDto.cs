public class GiftCardGetDto
    {
        public int page_nr { get; set; }
        public int limit { get; set; }
        public int original_amount_atleast { get; set; }
        public int original_amount_lessthan { get; set; } //TO-DO inconsistent grammar in api contract
        public decimal amount_left_atleast { get; set; }
        public decimal amount_left_lessthan { get; set; }
        public DateTime valid_starting_from { get; set; }
        public DateTime valid_atleast_until { get; set; }
        public string code_hash { get; set; } //TO-DO why is this here
    }