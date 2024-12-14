using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;
using Microsoft.CSharp.RuntimeBinder;
using PSPBackend.Dto;
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

    [HttpGet]
    [Route("")]
    public IActionResult GetOrders([FromQuery] OrderGetDto orderGetDto)
    {
        try
        {
            Console.WriteLine("LOG: Order controller GET request");
            List<OrderModel> gottenOrders = _orderService.GetOrders(orderGetDto);
            string stringed = "LOG: Order controller returns orders: ";
            foreach (OrderModel order in gottenOrders){
                stringed += order.ToString();
            }
            Console.WriteLine(stringed);
            return Ok(gottenOrders);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{order_id}")]
    public IActionResult GetOrder(int order_id)
    {
        
        try
        {
            Console.WriteLine("LOG: Order controller GET request");
            OrderModel? gottenOrder = _orderService.GetOrder(order_id);
            if(gottenOrder != null)
            {
                Console.WriteLine("LOG: Order controller returns order: " + gottenOrder);
                return Ok(gottenOrder);
            }
            else return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody] OrderModel order)
    {
        
        try
        {
            var result = _orderService.CreateOrder(order);
            if(result != null) return StatusCode(StatusCodes.Status201Created, result);
            return BadRequest();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{order_id}/status")]
    public IActionResult UpdateOrderStatus(int order_id, [FromBody] string body)
    {
        try
        {
            var returnOrder = _orderService.UpdateOrderStatus(order_id, body);
            if(returnOrder != null) return Ok(returnOrder);
            return UnprocessableEntity();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    [Route("{order_id}")]
    public IActionResult DeleteOrder(int order_id)
    {
        
        try
        {
            _orderService.DeleteOrder(order_id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{order_id}/orderItem")]
    public IActionResult AddItemToOrder(int order_id, [FromBody] OrderItemCreateDto item)
    {

        try
        {
            var result = _orderService.AddItem(order_id, item);
            if(result != null) return StatusCode(StatusCodes.Status201Created, result);
            return BadRequest();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{order_id}/orderItem")]
    public IActionResult GetOrderItems(int order_id, [FromQuery] int? page_nr, [FromQuery] int? limit)
    {
        try
        {
            var result = _orderService.GetItems(order_id, page_nr, limit);
            if(result != null) return Ok(result);
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{order_id}/orderItem/{item_id}")]
    public IActionResult GetOrderItem(int order_id, int item_id)
    {
        try
        {
            var result = _orderService.GetItem(order_id, item_id);
            if(result != null) return Ok(result);
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch]
    [Route("{order_id}/orderItem/{item_id}")]
    public IActionResult UpdateOrderItem(int order_id, int item_id, [FromBody] string body)
    {
        try
        {
            OrderItemUpdateDto? updateDto = JsonConvert.DeserializeObject<OrderItemUpdateDto>(body);
            var result = _orderService.UpdateItem(order_id, item_id, updateDto);
            if(result != null) return Ok(result);
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    [Route("{order_id}/orderItem/{item_id}")]
    public IActionResult DeleteOrderItem(int order_id, int item_id)
    {
        try
        {
            _orderService.DeleteItem(order_id, item_id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}