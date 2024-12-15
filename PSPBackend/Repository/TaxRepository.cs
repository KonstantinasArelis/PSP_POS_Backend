using PSPBackend.Model;

public class TaxRepository
{
    private readonly AppDbContext _context;

    public TaxRepository(AppDbContext context)
    {
        _context = context;  
    }

    public IQueryable<TaxModel> GetTaxes(bool? isValid)
    {
        var query = _context.Tax.AsQueryable();
        if(isValid.HasValue)
            query = query.Where(d => d.is_valid == isValid);
        return query; 
    }
    public int CreateTax(TaxModel tax)
    {
        _context.Tax.Add(tax);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }
    public TaxModel? GetTax(int taxId)
    {
        return _context.Tax.FirstOrDefault(d => d.id == taxId);
    }
    public int UpdateTax(int taxId, TaxModel tax)
    {
        TaxModel? oldTax = GetTax(taxId);
        if(oldTax != null) 
        {
            oldTax.tax_name = tax.tax_name;
            oldTax.tax_rate = tax.tax_rate;
            oldTax.is_valid = tax.is_valid;

            int rowsAffected = _context.SaveChanges(); 
            return rowsAffected;
        }
    
        return 0;
    }

    public int GetNewTaxId()
    {
        return _context.Tax.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}