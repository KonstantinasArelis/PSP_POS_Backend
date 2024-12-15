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
            _logger.LogError("OrderController encountered a problem in GetOrders (returning status 500) | " + e.Message);
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
            else 
            {
                _logger.LogError("OrderController encountered a problem in GetOrder (returning status 404)");
                return NotFound();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in GetOrder (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in CreateOrder (returning status 400)"); 
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in CreateOrder (returning status 500) | " + e.Message);
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
                _logger.LogInformation("OrderController successfully executed UpdateOrderStatus");
                return Ok(returnOrder);
            } 
            _logger.LogError("OrderController encountered a problem in UpdateOrderStatus (returning status 422)");
            return UnprocessableEntity();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in UpdateOrderStatus (returning status 500) | " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("{order_id}/discountPercentage")]
    public IActionResult UpdateOrderDiscount(int order_id, [FromBody] string body)
    {
        try
        {
            _orderService.UpdateOrderDiscount(order_id, body);
            _logger.LogInformation("OrderController successfully executed UpdateOrderDiscount");
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in UpdateOrderDiscount (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in DeleteOrder (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in AddItemToOrder (returning status 404)");
            return BadRequest();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in AddItemToOrder (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in GetOrderItems (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in GetOrderItem (returning status 404)");
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in GetOrderItem (returning status 500) | " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPatch]
    [Route("{order_id}/orderItem/{item_id}")]
    public IActionResult UpdateOrderItem(int order_id, int item_id, [FromBody] OrderItemUpdateDto updateDto)
    {
        try
        {
            var result = _orderService.UpdateItem(order_id, item_id, updateDto);
            if(result != null)
            {
                _logger.LogInformation("OrderController successfully executed UpdateOrderItem");
                return Ok(result);
            }
            _logger.LogError("OrderController encountered a problem in UpdateOrderItem (returning status 404)");
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderController encountered a problem in UpdateOrderItem (returning status 500) | " + e.Message);
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
            _logger.LogError("OrderController encountered a problem in DeleteOrderItem (returning status 500) | " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}