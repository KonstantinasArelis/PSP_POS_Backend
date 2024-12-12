using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;


[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Route("")]
    public List<OrderModel> GetOrders([FromQuery] OrderGetDto orderGetDto)
    {
        Console.WriteLine("LOG: Order controller GET request");
        List<OrderModel> gottenOrders = _orderService.GetOrders(orderGetDto);
        string stringed = "LOG: Order controller returns orders: ";
        foreach (OrderModel order in gottenOrders){
            stringed += order.ToString();
        }
        Console.WriteLine(stringed);
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
    public OrderModel? CreateOrder([FromBody] string body)
    {
        //string json_string = new StreamReader(Request.Body).ReadToEnd();
        Console.WriteLine("|||||||||||||||||creting order");
        OrderModel? order = JsonConvert.DeserializeObject<OrderModel>(body);
        if(order != null) return _orderService.CreateOrder(order);
        return null;
    }

    [HttpPost]
    [Route("{order_id}/status")]
    public void UpdateOrderStatus(int order_id, [FromBody] string body)
    {
        //string bodyString = new StreamReader(Request.Body).ReadToEnd();
        _orderService.UpdateOrderStatus(order_id, body);
    }

    [HttpDelete]
    [Route("{order_id}")]
    public void DeleteOrder(int order_id)
    {
        _orderService.DeleteOrder(order_id);
    }

    [HttpPost]
    [Route("{order_id}/orderItem")]
    public OrderItemModel? AddItemToOrder(int order_id, [FromBody] string body)
    {
        //string json_string = new StreamReader(Request.Body).ReadToEnd();
        OrderItemModel? item = JsonConvert.DeserializeObject<OrderItemModel>(body);
        if(item != null)
            return _orderService.AddItem(order_id, item);
        return null;
    }

    [HttpGet]
    [Route("{order_id}/orderItem")]
    public IEnumerable<OrderItemModel> GetOrderItems(int order_id, [FromQuery] int? page_nr, [FromQuery] int? limit)
    {
        return _orderService.GetItems(order_id, page_nr, limit);
    }

    [HttpGet]
    [Route("{order_id}/orderItem/{item_id}")]
    public OrderItemModel? GetOrderItem(int order_id, int item_id)
    {
        return _orderService.GetItem(order_id, item_id);
    }

    [HttpPatch]
    [Route("{order_id}/orderItem/{item_id}")]
    public void UpdateOrderItem(int order_id, int item_id, [FromBody] string body)
    {
        //string bodyString = new StreamReader(Request.Body).ReadToEnd();
        _orderService.UpdateItem(order_id, item_id, body);
    }

    [HttpDelete]
    [Route("{order_id}/orderItem/{item_id}")]
    public void DeleteOrderItem(int order_id, int item_id)
    {
        _orderService.DeleteItem(order_id, item_id);
    }

}