using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class RefundRepository
{
    private readonly AppDbContext _context;

    public RefundRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<RefundModel> GetRefunds(RefundGetDto refundGetDto)
    {
        var query = _context.Refund.AsQueryable(); // Assuming "Refund" is your DbSet

        if (refundGetDto.order_item_id != null)
            query = query.Where(r => r.order_item_id == refundGetDto.order_item_id);
        if (refundGetDto.returned_to_inventory != null)
            query = query.Where(r => r.returned_to_inventory == refundGetDto.returned_to_inventory);
        if (refundGetDto.created_before != null)
            query = query.Where(r => r.created_at <= refundGetDto.created_before);
        if (refundGetDto.created_after != null)
            query = query.Where(r => r.created_at >= refundGetDto.created_after);

        return query;
    }

    public RefundModel GetRefundById(int refundId)
    {
        RefundModel? gottenRefund = _context.Refund.Single(r => r.id == refundId);
        return gottenRefund;
    }

    public int CreateRefund(RefundModel newRefund)
    {
        _context.Refund.Add(newRefund);
        int rowsAffected = _context.SaveChanges();
        return rowsAffected;
    }

    public int GetNewRefundId()
    {
        return _context.Refund.Select(r => r.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}