using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class OrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;  

    }

    public OrderModel? AddOrder(OrderModel order)
    {
        _context.Order.Add(order);
        _context.SaveChanges();
        return _context.Order.Find(order.id); 
    }

    public IQueryable<OrderModel> GetOrders(OrderArgumentModel arguments) 
    {
        var query = _context.Order.AsQueryable();

        
        if (!string.IsNullOrEmpty(arguments.EmployeeId))
            query = query.Where(o => o.employee_id == arguments.EmployeeId);
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
        if (arguments.OrderStatus != null)
            query = query.Where(o => o.order_status == arguments.OrderStatus);
        Console.WriteLine("LOG: repository returns orders");
        return query; 
    }

    public OrderModel? GetOrder(int id)
    {
        return _context.Order.Include(o => o.items).FirstOrDefault(o => o.id == id);
    }

    public void DeleteOrder(int id)
    {
        OrderModel? order = _context.Order.Include(o => o.items).FirstOrDefault(o => o.id == id);
        if (order != null)
        {
            if(order.items != null)
            {
                foreach(OrderItemModel item in order.items)
                {
                    _context.Remove(item);
                }
            }
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
        Console.WriteLine("LOG: repository updates an order");
        return;
    }

    public OrderItemModel? AddOrderItem(OrderItemModel item)
    {
        _context.OrderItem.Add(item);
        _context.SaveChanges();
        return _context.OrderItem.Find(item.id); 
    }

    public OrderItemModel? GetOrderItem(int id)
    {
        return _context.OrderItem.FirstOrDefault(o => o.id == id);
    }

    public IQueryable<OrderItemModel> GetOrderItems(int id)
    {
        return _context.OrderItem.Where(o => o.order_id == id);
    }

    public void DeleteOrderItem(int id)
    {
        OrderItemModel? order = _context.OrderItem.Find(id);
        if (order != null)
        {
            _context.Remove(order);
            _context.SaveChanges();
            Console.WriteLine("LOG: repository deletes an order item");
            return;
        }
    }

    public void UpdateOrderItem(OrderItemModel itemToUpdate)
    {
        _context.OrderItem.Entry(itemToUpdate).CurrentValues.SetValues(itemToUpdate);
        _context.SaveChanges();
        Console.WriteLine("LOG: repository updates an order item");
        return;
    }

    public int GetNewOrderId()
    {
        return _context.Order.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
    
    public int GetNewOrderItemId()
    {
        return _context.OrderItem.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }

    public OrderItemModel getOrderItemByReservationId(int reservationId)
    {
        OrderItemModel result = _context.OrderItem.Single(c => c.reservation_id == reservationId);
        return result;
    } 

    public int closeOrder(int orderId)
    {
        OrderModel order = this.GetOrder(orderId);
        order.order_status = "CLOSED";
        int result = _context.SaveChanges();
        return result;
    }
}