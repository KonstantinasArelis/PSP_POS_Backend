using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class DiscountController : ControllerBase
{
    private readonly DiscountService _discountService;

    public DiscountController(DiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet]
    public IActionResult GetDiscounts(
        [FromQuery] int page_nr = 0, [FromQuery] int limit = 20, [FromQuery] string? name = null, [FromQuery] string? type = null, 
        [FromQuery] DateTime? valid_starting_from = null, [FromQuery] DateTime? valid_atleast_until = null, 
        [FromQuery] string? code_hash = null
        ) 
    {
        Console.WriteLine("LOG: Discount controller GET request");
        List<DiscountModel> gottenDiscounts = _discountService.GetDiscounts(
            page_nr, limit, name, type, 
            valid_starting_from, valid_atleast_until, 
            code_hash
            );
        Console.WriteLine("LOG: Discount controller returns Discounts: " + gottenDiscounts);
        return Ok(gottenDiscounts);
    }

    [HttpGet("{discountId}")]
    public IActionResult GetDiscountById(int discountId) 
    {
        Console.WriteLine("LOG: Discount controller GET GetDiscountById request");
        DiscountModel? gottenDiscount = _discountService.GetDiscount(discountId);
        Console.WriteLine("LOG: Discount controller returns Discount: " + gottenDiscount);
        if(gottenDiscount is not null)
            return Ok(gottenDiscount);
        else
            return StatusCode(500, "An error occured while getting discount"); // to be changed
    }

    [HttpDelete("{discountId}")]
    public IActionResult DeleteDiscount(int discountId) 
    {
        Console.WriteLine("LOG: Discount controller DELETE DeleteDiscount request");
        if (_discountService.DeleteDiscount(discountId) != null){
            return Ok();
        } else {
            return StatusCode(501); // http code to be changed
        }
    }

    [HttpPatch("{discountId}")]
    public IActionResult UpdateDiscount(int discountId, [FromBody] DiscountModel updatedDiscount) // case when foreign keys are not apprpriate should be considered
    {
        Console.WriteLine("LOG: Discount controller UPDATE UpdateDiscount request");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("UpdateDiscount invalid model");
            return BadRequest("Invalid data provided.");
        }

        var result = _discountService.UpdateDiscount(discountId, updatedDiscount);
        if (result == null)
        {
            return NotFound($"Discount with Id {discountId} not found.");
        }
        
        return Ok(result);

    }

    [HttpPost]
    public IActionResult CreateDiscount([FromBody] DiscountModel discount) // case when foreign keys are not apprpriate should be considered
    {    
        if(!ModelState.IsValid) 
        {
            Console.WriteLine("CreateDiscount invalid model");
            return BadRequest();
        } else {
            Console.WriteLine("CreateDiscount controller");

            DiscountModel? createdDiscount = _discountService.CreateDiscount(discount);
            if (createdDiscount != null){
                return Ok(createdDiscount);
            } else {
                return StatusCode(501); // http code to be changed(?)
            }
        }
        
        
    }
}