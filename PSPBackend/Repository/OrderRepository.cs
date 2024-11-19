using Microsoft.EntityFrameworkCore;
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

    public IQueryable<OrderModel> GetOrders(OrderArgumentModel arguments, int? orderStatus) 
    {
        var query = _context.Order.AsQueryable();

        
        if (arguments.EmployeeId.HasValue)
            query = query.Where(o => o.employee_id == arguments.EmployeeId.Value);
        if (arguments.MinTotalAmount.HasValue)
            query = query.Where(o => o.total_amount >= arguments.MinTotalAmount.Value);
        if (arguments.MaxTotalAmount.HasValue)
            query = query.Where(o => o.total_amount <= arguments.MaxTotalAmount.Value);
        /*if (arguments.MinTipAmount.HasValue)
            query = query.Where(o => o >= arguments.MinTipAmount.Value);
        if (arguments.MaxTipAmount.HasValue)
            query = query.Where(o => o.total_amount <= arguments.MaxTipAmount.Value);*/
        if (arguments.MinTaxAmount.HasValue)
            query = query.Where(o => o.tax_amount >= arguments.MinTaxAmount.Value);
        if (arguments.MaxTaxAmount.HasValue)
            query = query.Where(o => o.tax_amount <= arguments.MaxTaxAmount.Value);
        if (arguments.MinDiscountAmount.HasValue)
            query = query.Where(o => o.total_discount_amount >= arguments.MinDiscountAmount.Value);
        if (arguments.MaxDiscountAmount.HasValue)
            query = query.Where(o => o.total_discount_amount <= arguments.MaxDiscountAmount.Value);
        if (arguments.MinOrderDiscountPercentage.HasValue)
            query = query.Where(o => o.order_discount_percentage >= arguments.MinOrderDiscountPercentage.Value);
        if (arguments.MaxOrderDiscountPercentage.HasValue)
            query = query.Where(o => o.order_discount_percentage <= arguments.MaxOrderDiscountPercentage.Value);
        if (arguments.CreatedBefore!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.CreatedBefore);
                query = query.Where(o => o.created_at <= date);
            }
            catch(FormatException){}
        }
        if (arguments.CreatedAfter!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.CreatedAfter);
                query = query.Where(o => o.created_at >= date);
            }
            catch(FormatException){}
        }
        if (arguments.ClosedBefore!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.ClosedBefore);
                query = query.Where(o => o.closed_at <= date);
            }
            catch(FormatException){}
        }
        if (arguments.ClosedAfter!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.ClosedAfter);
                query = query.Where(o => o.closed_at >= date);
            }
            catch(FormatException){}
        }
        if (orderStatus.HasValue)
            query = query.Where(o => o.order_status == orderStatus.Value);
        Console.WriteLine("LOG: repository returns orders");
        return query; 
    }

    public OrderModel? GetOrder(int id)
    {
        return _context.Order.Include(o => o.items).First(o => o.id == id);
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