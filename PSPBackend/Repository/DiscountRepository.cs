using PSPBackend.Model;
using System.Linq;

public class DiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;  
    }

    public IQueryable<DiscountModel> GetDiscounts(string? name, string? type, DateTime? valid_starting_from,
            DateTime? valid_atleast_until, string? code_hash) 
    {
        var query = _context.Discount.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(d => d.discount_name == name);

        if (!string.IsNullOrEmpty(type))
            query = query.Where(d => d.discount_type == type);

        if (valid_starting_from.HasValue)
            query = query.Where(d => d.valid_from >= valid_starting_from.Value); 
        
        if (valid_atleast_until.HasValue)
            query = query.Where(d => d.valid_until >= valid_atleast_until.Value);

        if (!string.IsNullOrEmpty(code_hash))
            query = query.Where(d => d.code_hash == code_hash);

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
        return 0; 
    }

    public int UpdateDiscount(int discountId, DiscountModel discount)
    {
        DiscountModel? oldDiscount = GetDiscount(discountId);
        if(oldDiscount != null) 
        {
            oldDiscount.business_id = discount.business_id;
            oldDiscount.product_id = discount.product_id;
            oldDiscount.discount_name = discount.discount_name;
            oldDiscount.discount_type = discount.discount_type;
            oldDiscount.amount = discount.amount;
            oldDiscount.discount_percentage = discount.discount_percentage;  
            oldDiscount.valid_from = discount.valid_from;  
            oldDiscount.valid_until = discount.valid_until;  
            oldDiscount.code_hash = discount.code_hash;  

            int rowsAffected = _context.SaveChanges(); 
            return rowsAffected;
        }
    
        return 0;
    }

    public int CreateDiscount(DiscountModel discount)
    {

        // constraints
        if(discount.discount_type == "ORDER" || discount.discount_type == "ORDER_ITEM")
        {
            if(string.IsNullOrEmpty(discount.code_hash))
            {
                return 0;
            }
            if(discount.product_id != null)
            {
                return 0;
            }
            if(discount.amount != null)
            {
                return 0;
            }
        }
        
        if(discount.discount_type == "PRODUCT")
        {
            if(discount.product_id == null)
            {
                return 0;
            }
            if(discount.amount == null && discount.discount_percentage == null)
            {
                return 0;
            }
        }
        _context.Discount.Add(discount);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }

    public int GetNewDiscountId()
    {
        return _context.Discount.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
    

}