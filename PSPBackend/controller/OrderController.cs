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
        [FromQuery] int? page_nr, [FromQuery] int? limit, [FromQuery] int? employee_id, 
        [FromQuery] decimal? min_total_amount, [FromQuery] decimal? max_total_amount, 
        [FromQuery] decimal? min_tip_amount, [FromQuery] decimal? max_tip_amount,
        [FromQuery] decimal? min_tax_amount, [FromQuery] decimal? max_tax_amount,
        [FromQuery] decimal? min_discount_amount, [FromQuery] decimal? max_discount_amount,
        [FromQuery] decimal? min_order_discount_percentage, [FromQuery] decimal? max_order_discount_percentage,
        [FromQuery] string? created_before, [FromQuery] string? created_after,
        [FromQuery] string? closed_before, [FromQuery] string? closed_after,
        [FromQuery] string? order_status
        ) 
    {
        Console.WriteLine("LOG: Order controller GET request");
        OrderArgumentModel arguments = new OrderArgumentModel(){
            PageNr = page_nr,
            Limit = limit,
            EmployeeId = employee_id,
            MinTotalAmount = min_total_amount,
            MaxTotalAmount = max_total_amount,
            MinTipAmount = min_tip_amount,
            MaxTipAmount = max_tip_amount,
            MinTaxAmount = min_tax_amount,
            MaxTaxAmount = max_tax_amount,
            MinDiscountAmount = min_discount_amount,
            MaxDiscountAmount = max_discount_amount,
            MinOrderDiscountPercentage = min_order_discount_percentage,
            MaxOrderDiscountPercentage = max_order_discount_percentage,
            CreatedBefore = created_before,
            CreatedAfter = created_after,
            ClosedBefore = closed_before,
            ClosedAfter = closed_after,
            OrderStatus = order_status
        };
        List<OrderModel> gottenOrders = _orderService.GetOrders(arguments);
        string stringed = "LOG: Order controller returns orders: ";
        foreach (OrderModel order in gottenOrders){
            stringed += order.ToString();
        }
        Console.WriteLine(stringed);
        return gottenOrders;
    }
    [HttpGet]
    [Route("{order_id}")]
    public ActionResult<OrderModel?> GetOrder(int order_id)
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
        OrderModel? order = JsonConvert.DeserializeObject<OrderModel>(json_string);
        if(order != null) _orderService.CreateOrder(order);
    }
    [HttpPost]
    [Route("{order_id}/status")]
    public void UpdateOrderStatus(int order_id)
    {
        string json_string = new StreamReader(Request.Body).ReadToEnd();
        dynamic? obj = JsonConvert.DeserializeObject<dynamic>(json_string);
        if(obj != null)
        {
            try
            {
                string s = obj.status;
                _orderService.UpdateOrder(_orderService.GetOrder(order_id), obj.status);
            }
            catch(RuntimeBinderException){}
        }
    }
}