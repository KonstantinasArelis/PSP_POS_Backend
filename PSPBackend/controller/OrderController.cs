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

    [HttpGet]
    public string Get()
    {
        Console.WriteLine("LOG: controller creates order");
        string temp = _orderService.CreateOrder();
        Console.WriteLine("LOG: controller supposed to return: " + temp);
        return temp;
    }
}