using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSPBackend.Model
{

    public class DiscountModel
    {
        [Key]
        public int id { get; set; }
       
        public int? business_id { get; set; } 

        public int? product_id { get; set; } //? specified in pdf

        public int? discount_type { get; set; }

        public decimal? amount { get; set; }

        public decimal? discount_percentage { get; set; } // DECIMAL(5,2) means 105.25 is theretically allowed - TO DO!

        public DateTime? valid_from { get; set; } //? specified in pdf

        public DateTime? valid_until { get; set; } //? specified in pdf

        public string? code_hash { get; set; } //? specified in pdf // what code_hash is for?????
    }
}
