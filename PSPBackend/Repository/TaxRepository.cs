using PSPBackend.Model;

public class TaxRepository
{
    private readonly AppDbContext _context;

    public TaxRepository(AppDbContext context)
    {
        _context = context;  

    }

}