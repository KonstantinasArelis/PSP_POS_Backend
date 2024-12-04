public class RefundGetDto
    {
        public int? page_nr { get; set; }
        public int? limit { get; set; }
        public int? order_item_id { get; set; }
        public bool? returned_to_inventory { get; set; }
        public DateTime? created_before {get; set; }
        public DateTime? created_after {get; set; }
    }