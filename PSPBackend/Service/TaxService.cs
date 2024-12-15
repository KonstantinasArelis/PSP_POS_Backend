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
            var query = _taxRepository.GetTaxes(isValid); 

            var taxes = query.Skip(page_nr * limit).Take(limit).ToList(); 
            return taxes;
        }

        public TaxModel? CreateTax(TaxModel tax)
        {            
            if(tax.id == 0)
            {
                tax.id = _taxRepository.GetNewTaxId();
            }

            if (_taxRepository.CreateTax(tax) > 0){
                return tax;
            } else {
                return null;
            }
        }

        public TaxModel? GetTax(int taxId)
        {
            
            var tax = _taxRepository.GetTax(taxId); 
            if(tax is null){
                return null;
            } else {
                return tax;
            }
        }

        public TaxModel? UpdateTax(int taxId, TaxModel tax)
        {
            
            if (_taxRepository.UpdateTax(taxId, tax) > 0){
                return tax;
            } else {
                return null;
            }
        }

}