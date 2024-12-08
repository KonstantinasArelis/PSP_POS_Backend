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
        Console.WriteLine("LOG: repository returns Taxes");
        return query; 
    }
    public int CreateTax(TaxModel tax)
    {
        Console.WriteLine("CreateTax repository");
        _context.Tax.Add(tax);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }
    public TaxModel? GetTax(int taxId)
    {
        Console.WriteLine("GetTax repository");
        return _context.Tax.FirstOrDefault(d => d.id == taxId);
    }

    // Modify or invalidate a tax. 
    public int UpdateTax(int taxId, TaxModel tax)
    {
        Console.WriteLine("UpdateDiscount repository");
        TaxModel? oldTax = GetTax(taxId);
        if(oldTax != null) 
        {
            oldTax.tax_name = tax.tax_name;
            oldTax.tax_rate = tax.tax_rate;
            oldTax.is_valid = tax.is_valid;

            int rowsAffected = _context.SaveChanges(); 
            return rowsAffected;
        }
    
        return 0; // maybe another way can be used???
    }

    public int GetNewTaxId()
    {
        return _context.Tax.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}