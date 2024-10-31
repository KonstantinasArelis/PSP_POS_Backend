namespace PSPBackend.Model
{
    public class TaxModel
    {
        public int Id { get; set; }
        public string TaxName { get; set; }
        public decimal TaxRate { get; set; }
        public bool IsValid { get; set; }
    }
}

