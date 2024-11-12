using PSPBackend.Model;
public class TaxService
{
        private readonly TaxRepository _taxRepository;

        public TaxService(TaxRepository taxRepository)
        {
            _taxRepository = taxRepository;  

        }

}