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
        [FromQuery] int page_nr, [FromQuery] int limit, [FromQuery] int type, 
        [FromQuery] DateTime? valid_starting_from, [FromQuery] DateTime? valid_atleast_until, 
        [FromQuery] string? code_hash
        ) // changing string to int for "type" so that it would correspond w/ data model, even though .yaml has it as string type
    {
        Console.WriteLine("LOG: Discount controller GET request");
        List<DiscountModel> gottenDiscounts = _discountService.GetDiscounts(
            page_nr, limit, type, 
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
    public IActionResult UpdateDiscount(int discountId, [FromBody] DiscountModel updatedDiscount) 
    {
        Console.WriteLine("LOG: Discount controller DELETE UpdateDiscount request");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("UpdateDiscount invalid model");
            return BadRequest("Invalid data provided.");
        }

        // Call the service to update the discount
        var result = _discountService.UpdateDiscount(discountId, updatedDiscount);
        if (result == null)
        {
            return NotFound($"Discount with Id {discountId} not found.");
        }
        
        return Ok(result);

    }

    [HttpPost]
    public IActionResult CreateDiscount([FromBody] DiscountModel discount)
    {
        Console.WriteLine("CreateDiscount controller");
        if(!ModelState.IsValid) // what does this check??
        {
            Console.WriteLine("CreateDiscount invalid model");
            return BadRequest();
        } else {
            if (_discountService.CreateDiscount(discount) != null){
                return Ok();
            } else {
                return StatusCode(501); // http code to be changed(?)
            }
        }
        
        
    }
}