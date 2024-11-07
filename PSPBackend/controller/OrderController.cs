using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    /*
    [HttpGet]
    public string Get()
    {
        Console.WriteLine("LOG: controller creates order");
        string temp = _orderService.CreateOrder();
        Console.WriteLine("LOG: controller supposed to return: " + temp);
        return temp;
    }
    */
    [HttpGet]
    public List<OrderModel> Get(
        [FromQuery] int page_nr, [FromQuery] int limit, [FromQuery] int? employee_id, 
        [FromQuery] decimal? min_total_amount, [FromQuery] decimal? max_total_amount, 
        [FromQuery] string order_status
        ) 
    {
        Console.WriteLine("LOG: Order controller GET request");
        List<OrderModel> gottenOrders = _orderService.GetOrders(
            page_nr, limit, employee_id, 
            min_total_amount, max_total_amount, 
            order_status
            );
        Console.WriteLine("LOG: Order controller returns orders: " + gottenOrders);
        return gottenOrders;
    }
}