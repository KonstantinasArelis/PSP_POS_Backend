using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class DiscountController : ControllerBase
{
    private readonly DiscountService _discountService;
    private readonly ILogger<DiscountController> _logger;

    public DiscountController(DiscountService discountService, ILogger<DiscountController> logger)
    {
        _discountService = discountService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetDiscounts(
        [FromQuery] int page_nr = 0, [FromQuery] int limit = 20, [FromQuery] string? name = null, [FromQuery] string? type = null, 
        [FromQuery] DateTime? valid_starting_from = null, [FromQuery] DateTime? valid_atleast_until = null, 
        [FromQuery] string? code_hash = null
        ) 
    {
        List<DiscountModel> gottenDiscounts = _discountService.GetDiscounts(
            page_nr, limit, name, type, 
            valid_starting_from, valid_atleast_until, 
            code_hash
            );
        _logger.LogInformation("DiscountController successfully executed GetDiscounts");
        return Ok(gottenDiscounts);
    }

    [HttpGet("{discountId}")]
    public IActionResult GetDiscountById(int discountId) 
    {
        DiscountModel? gottenDiscount = _discountService.GetDiscount(discountId);
        if(gottenDiscount is not null)
        {
            _logger.LogInformation("DiscountController successfully executed GetDiscountById");
            return Ok(gottenDiscount);
        }
        else
        {
            _logger.LogError("DiscountController encountered a problem in GetDiscountById (returning status 500)");
            return StatusCode(500, "An error occured while getting discount"); // to be changed
        }
    }

    [HttpDelete("{discountId}")]
    public IActionResult DeleteDiscount(int discountId) 
    {
        if (_discountService.DeleteDiscount(discountId) != null){
            _logger.LogInformation("DiscountController successfully executed DeleteDiscount");
            return Ok();
        } else {
            _logger.LogError("DiscountController encountered a problem in DeleteDiscount (returning status 501)");
            return StatusCode(501); // http code to be changed
        }
    }

    [HttpPatch("{discountId}")]
    public IActionResult UpdateDiscount(int discountId, [FromBody] DiscountModel updatedDiscount) // case when foreign keys are not apprpriate should be considered
    {        
        if (!ModelState.IsValid)
        {
            _logger.LogError("DiscountController encountered a problem in UpdateDiscount (returning status 400)");
            return BadRequest("Invalid data provided.");
        }

        var result = _discountService.UpdateDiscount(discountId, updatedDiscount);
        if (result == null)
        {
            _logger.LogError("DiscountController encountered a problem in UpdateDiscount (returning status 404)");
            return NotFound($"Discount with Id {discountId} not found.");
        }
        _logger.LogInformation("DiscountController successfully executed UpdateDiscount");
        return Ok(result);

    }

    [HttpPost]
    public IActionResult CreateDiscount([FromBody] DiscountModel discount) // case when foreign keys are not apprpriate should be considered
    {    
        if(!ModelState.IsValid) 
        {
            _logger.LogError("DiscountController encountered a problem in GetDiscountById (returning status 400)");
            return BadRequest();
        } else {
            DiscountModel? createdDiscount = _discountService.CreateDiscount(discount);
            if (createdDiscount != null){
                _logger.LogInformation("DiscountController successfully executed CreateDiscount");
                return Ok(createdDiscount);
            } else {
                _logger.LogError("DiscountController encountered a problem in CreateDiscount (returning status 501)");
                return StatusCode(501); // http code to be changed(?)
            }
        }
        
        
    }
}