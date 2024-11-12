using PSPBackend.Model;

public class OrderStatusRepository
{
    private readonly AppDbContext _context;

    public OrderStatusRepository(AppDbContext context)
    {
        _context = context;  

    }

    public IQueryable<OrderStatusModel> GetAllOrderStatuses() 
    {
        return _context.OrderStatus.AsQueryable();
    }

    public OrderStatusModel? GetOrderStatusById(int id)
    {
        return _context.OrderStatus.Find(id);
    }

    public OrderStatusModel? GetOrderStatusByName(string name)
    {
        return _context.OrderStatus.Where(o => o.OrderStatusName == name).First();
    }

    public int? ConvertNameToCode(string name)
    {
        if(name == null) return null;
        return _context.OrderStatus.Where(o => o.OrderStatusName == name).First().OrderStatusId;
    }
}