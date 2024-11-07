using PSPBackend.Model;

public class OrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;  

    }

    public string AddOrder(OrderModel order)
    {
        _context._Order.Add(order);
        _context.SaveChanges();
        return "201 all good"; 
    }

    public IQueryable<OrderModel> GetOrders(int? employee_id, decimal? min_total_amount, 
                                       decimal? max_total_amount, string order_status) 
    {
        var query = _context._Order.AsQueryable();

        /*
        if (employee_id.HasValue)
            query = query.Where(o => o.id == employee_id.Value);
        */
        if (min_total_amount.HasValue)
            query = query.Where(o => o.total_amount >= min_total_amount.Value);

        if (max_total_amount.HasValue)
            query = query.Where(o => o.total_amount <= max_total_amount.Value);
        Console.WriteLine("LOG: repository returns orders");
        return query; 
    }
}