using PSPBackend.Model;

public class OrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;  

    }

    // Store data
    public string AddOrder(OrderModel order)
    {
        _context._Order.Add(order);
        _context.SaveChanges();
        return "201 all good"; 
    }

    // Retrieve data
    public List<OrderModel> GetAllProducts()
    {
        return _context._Order.ToList();
    }

    // Example of retrieving a single product by ID
    public OrderModel GetProductById(int id)
    {
        return _context._Order.Find(id); 
    }
}