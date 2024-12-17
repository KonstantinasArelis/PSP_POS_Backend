namespace PSPBackend.Model
{
    public class TaxModel
    {
        public int id { get; set; }
        public string? tax_name { get; set; }
        public decimal? tax_rate { get; set; }
        public bool? is_valid { get; set; }
    }
}

