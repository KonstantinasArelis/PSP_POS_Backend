using PSPBackend.Model;
using System.Linq;

public class DiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;  
    }

    public IQueryable<DiscountModel> GetDiscounts(int? type, DateTime? valid_starting_from,
            DateTime? valid_atleast_until, string? code_hash) // do we really need page_nr and limit? And how is this supposed to work??
    {
        var query = _context.Discount.AsQueryable();

        if (type.HasValue)
            query = query.Where(d => d.discount_type == type);

        if (valid_starting_from.HasValue)
            query = query.Where(d => d.valid_from >= valid_starting_from.Value); // should it be like this??
        
        if (valid_atleast_until.HasValue)
            query = query.Where(d => d.valid_until >= valid_atleast_until.Value); // should it be like this??

        if (!string.IsNullOrEmpty(code_hash))
            query = query.Where(d => d.code_hash == code_hash);

        Console.WriteLine("LOG: repository returns Discounts");
        return query; 
    }

    public DiscountModel? GetDiscount(int discountId)
    {
        return _context.Discount.FirstOrDefault(d => d.id == discountId);
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
            oldDiscount.business_id = discount.business_id;
            oldDiscount.product_id = discount.product_id;
            oldDiscount.discount_type = discount.discount_type;
            oldDiscount.amount = discount.amount;
            oldDiscount.discount_percentage = discount.discount_percentage;  
            oldDiscount.valid_from = discount.valid_from;  
            oldDiscount.valid_until = discount.valid_until;  
            oldDiscount.code_hash = discount.code_hash;  

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