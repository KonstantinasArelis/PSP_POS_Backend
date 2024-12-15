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
    private readonly ILogger<OrderController> _logger;

    public OrderController(OrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetOrders([FromQuery] OrderGetDto orderGetDto)
    {
        try
        {
            List<OrderModel> gottenOrders = _orderService.GetOrders(orderGetDto);
            _logger.LogInformation("OrderController successfully executed GetOrders");
            return Ok(gottenOrders);
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered an exception in GetOrders: " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{order_id}")]
    public IActionResult GetOrder(int order_id)
    {
        
        try
        {
            OrderModel? gottenOrder = _orderService.GetOrder(order_id);
            if(gottenOrder != null)
            {
                _logger.LogInformation("OrderController successfully executed GetOrder");
                return Ok(gottenOrder);
            }
            else return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered an exception in GetOrder: " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody] OrderModel order)
    {
        
        try
        {
            var result = _orderService.CreateOrder(order);
            if(result != null)
            {
                _logger.LogInformation("OrderController successfully executed CreateOrder");
                return StatusCode(StatusCodes.Status201Created, result);
            } 
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered an exception in CreateOrder: " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{order_id}/status")]
    public IActionResult UpdateOrderStatus(int order_id, [FromBody] OrderStatusDto status)
    {
        try
        {
            var returnOrder = _orderService.UpdateOrderStatus(order_id, status.status);
            if(returnOrder != null)
            {
                _logger.LogInformation("OrderController successfully executed UpdateOrder");
                return Ok(returnOrder);
            } 
            return UnprocessableEntity();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in UpdateOrderStatus: " + e.Message);
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
            _logger.LogInformation("OrderController successfully executed DeleteOrder");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in DeleteOrder: " + e.Message);
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
            if(result != null)
            {
                _logger.LogInformation("OrderController successfully executed AddItemToOrder");
                return StatusCode(StatusCodes.Status201Created, result);
            } 
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in AddItemToOrder: " + e.Message);
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
            _logger.LogInformation("OrderController successfully executed GetOrderItems");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in GetOrderItems: " + e.Message);
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
            if(result != null)
            {
                _logger.LogInformation("OrderController successfully executed GetOrderItem");
                return Ok(result);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in GetOrderItem: " + e.Message);
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
            if(result != null)
            {
                _logger.LogInformation("OrderController successfully executed UpdateOrderItem");
                return Ok(result);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in UpdateOrderItem: " + e.Message);
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
            _logger.LogInformation("OrderController successfully executed DeleteOrderItem");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController got an exception in DeleteOrderItem: " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}