using System.ComponentModel.DataAnnotations;

namespace PSPBackend.Model
{

    public class DiscountModel
    {
        [Key]
        public int id { get; set; }
       
        public int? business_id { get; set; } 

        public int? product_id { get; set; }

        public string? discount_type { get; set; }

        public decimal? amount { get; set; }

        public decimal? discount_percentage { get; set; } 

        public DateTime? valid_from { get; set; } 

        public DateTime? valid_until { get; set; }

        public string? code_hash { get; set; } // what code_hash is for?????
    }
}
