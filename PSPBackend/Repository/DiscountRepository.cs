using PSPBackend.Model;
using System.Linq;

public class DiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;  

    }

    public IQueryable<DiscountModel> GetDiscounts(int type, DateTime? valid_starting_from,
            DateTime? valid_atleast_until, string? code_hash) // do we really need page_nr and limit? And how is this supposed to work??
    {
        var query = _context.Discount.AsQueryable();

        query = query.Where(d => d.DiscountType == type);

        if (valid_starting_from.HasValue)
        {
            query = query.Where(d => d.ValidFrom >= valid_starting_from.Value); // should it be like this??
        }
        
        if (valid_atleast_until.HasValue)
        {
            query = query.Where(d => d.ValidUntil >= valid_atleast_until.Value); // should it be like this??
        }

        if (!string.IsNullOrEmpty(code_hash))
        {
            query = query.Where(d => d.CodeHash == code_hash);
        }

        Console.WriteLine("LOG: repository returns Discounts");
        return query; 
    }

    public DiscountModel? GetDiscount(int discountId)
    {
        return _context.Discount.FirstOrDefault(d => d.Id == discountId);
    }
    
    public int DeleteDiscount(int discountId)
    {
        DiscountModel? discount = GetDiscount(discountId);
        if(discount != null) 
        {
            _context.Discount.Remove(discount);
            int rowsAffected = _context.SaveChanges(); 
            return rowsAffected;
        }
        return 0; // maybe another way can be used???
    }

    public int UpdateDiscount(int discountId, DiscountModel discount)
    {
        Console.WriteLine("UpdateDiscount repository");
        DiscountModel? oldDiscount = GetDiscount(discountId);
        if(oldDiscount != null) 
        {
            oldDiscount.Id = discount.Id;
            oldDiscount.BusinessId = discount.BusinessId;
            oldDiscount.ProductId = discount.ProductId;
            oldDiscount.DiscountType = discount.DiscountType;
            oldDiscount.Amount = discount.Amount;
            oldDiscount.DiscountPercentage = discount.DiscountPercentage;  
            oldDiscount.ValidFrom = discount.ValidFrom;  
            oldDiscount.ValidUntil = discount.ValidUntil;  
            oldDiscount.CodeHash = discount.CodeHash;  

            int rowsAffected = _context.SaveChanges(); 
            return rowsAffected;
        }
    
        return 0; // maybe another way can be used???
    }

    public int CreateDiscount(DiscountModel discount)
    {
        Console.WriteLine("CreateDiscount repository");
        _context.Discount.Add(discount);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }

}