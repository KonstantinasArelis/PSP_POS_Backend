namespace PSPBackend.Model
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int BusinessId { get; set; }
        public decimal Price { get; set; }
        public int ProductType { get; set; }
        public bool IsForSale { get; set; }
        public int TaxId { get; set; }
        public int CategoryId { get; set; }
        public bool CanDiscountBeApplied { get; set; }
        public bool StockQuantity { get; set; }
        public string Variations { get; set; }
    }
}
