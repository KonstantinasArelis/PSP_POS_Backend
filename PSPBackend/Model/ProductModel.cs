using System.ComponentModel.DataAnnotations.Schema;

namespace PSPBackend.Model
{
    public class ProductModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("business_id")]
        public int BusinessId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("product_type")]
        public string ProductType { get; set; } // SERVICE or ITEM or SERVICE_CHARGE
        [Column("is_for_sale")]
        public bool IsForSale { get; set; }

        [Column("tax_id")]
        public int TaxId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("can_discount_be_applied")]
        public bool CanDiscountBeApplied { get; set; }

        [Column("stock_quantity")]
        public bool StockQuantity { get; set; }

        [Column("variations")]
        public string Variations { get; set; }
    }
}
