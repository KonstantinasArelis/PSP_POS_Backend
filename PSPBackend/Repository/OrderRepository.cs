using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Dto;
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

    public IQueryable<OrderModel> GetOrders(OrderGetDto arguments) 
    {
        var query = _context.Order.AsQueryable();

        
        if (arguments.employee_id.HasValue)
            query = query.Where(o => o.employee_id == arguments.employee_id.Value);
        if (arguments.min_total_amount.HasValue)
            query = query.Where(o => o.total_amount >= arguments.min_total_amount.Value);
        if (arguments.max_total_amount.HasValue)
            query = query.Where(o => o.total_amount <= arguments.max_total_amount.Value);
        if (arguments.min_tax_amount.HasValue)
            query = query.Where(o => o.tax_amount >= arguments.min_tax_amount.Value);
        if (arguments.max_tax_amount.HasValue)
            query = query.Where(o => o.tax_amount <= arguments.max_tax_amount.Value);
        if (arguments.min_discount_amount.HasValue)
            query = query.Where(o => o.total_discount_amount >= arguments.min_discount_amount.Value);
        if (arguments.max_discount_amount.HasValue)
            query = query.Where(o => o.total_discount_amount <= arguments.max_discount_amount.Value);
        if (arguments.min_order_discount_percentage.HasValue)
            query = query.Where(o => o.order_discount_percentage >= arguments.min_order_discount_percentage.Value);
        if (arguments.max_order_discount_percentage.HasValue)
            query = query.Where(o => o.order_discount_percentage <= arguments.max_order_discount_percentage.Value);
        if (arguments.created_before!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.created_before);
                query = query.Where(o => o.created_at <= date);
            }
            catch(FormatException){}
        }
        if (arguments.created_after!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.created_after);
                query = query.Where(o => o.created_at >= date);
            }
            catch(FormatException){}
        }
        if (arguments.closed_before!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.closed_before);
                query = query.Where(o => o.closed_at <= date);
            }
            catch(FormatException){}
        }
        if (arguments.closed_after!= null)
        {
            try
            {
                DateTime date = DateTime.Parse(arguments.closed_after);
                query = query.Where(o => o.closed_at >= date);
            }
            catch(FormatException){}
        }
        if (arguments.order_status != null)
            query = query.Where(o => o.order_status == arguments.order_status);
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
            return;
        }
    }

    public OrderModel UpdateOrder(OrderModel orderToUpdate)
    {
        _context.Order.Entry(orderToUpdate).CurrentValues.SetValues(orderToUpdate);
        _context.SaveChanges();
        return orderToUpdate;
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
            return;
        }
    }

    public OrderItemModel? UpdateOrderItem(OrderItemModel itemToUpdate)
    {
        _context.OrderItem.Entry(itemToUpdate).CurrentValues.SetValues(itemToUpdate);
        _context.SaveChanges();
        return itemToUpdate;
    }

    public int GetNewOrderId()
    {
        return _context.Order.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
    
    public int GetNewOrderItemId()
    {
        return _context.OrderItem.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}