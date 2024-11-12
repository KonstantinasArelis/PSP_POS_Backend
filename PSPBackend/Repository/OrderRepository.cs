using PSPBackend.Model;

public class OrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;  

    }

    public int? AddOrder(OrderModel order)
    {
        _context.Order.Add(order);
        _context.SaveChanges();
        return _context.Order.Find(order.id)?.id; 
    }

    public IQueryable<OrderModel> GetOrders(int? employee_id, decimal? min_total_amount, 
                                       decimal? max_total_amount, int? order_status) 
    {
        var query = _context.Order.AsQueryable();

        
        if (employee_id.HasValue)
            query = query.Where(o => o.employee_id == employee_id.Value);
        if (min_total_amount.HasValue)
            query = query.Where(o => o.total_amount >= min_total_amount.Value);

        if (max_total_amount.HasValue)
            query = query.Where(o => o.total_amount <= max_total_amount.Value);
        if (order_status.HasValue)
            query = query.Where(o => o.order_status == order_status.Value);
        Console.WriteLine("LOG: repository returns orders");
        return query; 
    }

    public OrderModel? GetOrder(int id)
    {
        return _context.Order.Find(id);
    }

    public void DeleteOrder(int id)
    {
        OrderModel? order = _context.Order.Find(id);
        if (order != null)
        {
            _context.Remove(order);
            _context.SaveChanges();
            Console.WriteLine("LOG: repository deletes an order");
            return;
        }
    }

    public void UpdateOrder(OrderModel orderToUpdate)
    {
        _context.Order.Entry(orderToUpdate).CurrentValues.SetValues(orderToUpdate);
        _context.SaveChanges();
        Console.WriteLine("LOG: repository deletes an order");
        return;
    }
}