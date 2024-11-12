using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;
using Newtonsoft.Json;


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
    [Route("")]
    public List<OrderModel> GetOrders(
        [FromQuery] int page_nr, [FromQuery] int limit, [FromQuery] int? employee_id, 
        [FromQuery] decimal? min_total_amount, [FromQuery] decimal? max_total_amount, 
        [FromQuery] string? order_status
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
    [HttpGet]
    [Route("{order_id}")]
    public OrderModel? GetOrder(int order_id)
    {
        Console.WriteLine("LOG: Order controller GET request");
        OrderModel? gottenOrder = _orderService.GetOrder(order_id);
        Console.WriteLine("LOG: Order controller returns order: " + gottenOrder);
        return gottenOrder;
    }

    [HttpPost]
    public void CreateOrder()
    {
        string json_string = new StreamReader(Request.Body).ReadToEnd();
        OrderModel order = JsonConvert.DeserializeObject<OrderModel>(json_string);
        _orderService.CreateOrder(order);
    }
    [HttpPost]
    [Route("{order_id}/status")]
    public void UpdateOrderStatus(int order_id)
    {
        string json_string = new StreamReader(Request.Body).ReadToEnd();
        dynamic obj = JsonConvert.DeserializeObject<dynamic>(json_string);
        _orderService.UpdateOrder(_orderService.GetOrder(order_id), obj.status);
    }
}