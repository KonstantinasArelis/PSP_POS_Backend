using PSPBackend.Model;
public class TaxService
{
        private readonly TaxRepository _taxRepository;

        public TaxService(TaxRepository taxRepository)
        {
            _taxRepository = taxRepository;  

        }

         public List<TaxModel> GetTaxes(int page_nr, int limit, bool? isValid)
        {
            Console.WriteLine("LOG: Tax service GetTaxes");
            var query = _taxRepository.GetTaxes(isValid); 

            var taxes = query.Skip(page_nr * limit).Take(limit).ToList(); // Why like this???
            return taxes;
        }

        public TaxModel? CreateTax(TaxModel tax)
        {
            Console.WriteLine("CreateTax service");
            
            if (_taxRepository.CreateTax(tax) > 0){
                return tax;
            } else {
                return null;
            }
        }

        public TaxModel? GetTax(int taxId)
        {
            
            Console.WriteLine("LOG: Tax service GetTax");
            var tax = _taxRepository.GetTax(taxId); 
            if(tax is null){
                return null;
            } else {
                return tax;
            }
        }

        public TaxModel? UpdateTax(int taxId, TaxModel tax)
        {
            Console.WriteLine("UpdateTax service");
            
            if (_taxRepository.UpdateTax(taxId, tax) > 0){
                return tax;
            } else {
                return null;
            }
        }

}