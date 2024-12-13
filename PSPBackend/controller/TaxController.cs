using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class TaxController : ControllerBase
{
    private readonly TaxService _taxService;

    public TaxController(TaxService taxService)
    {
        _taxService = taxService;
    }

    [HttpGet]
    public IActionResult GetTaxes([FromQuery] int page_nr = 0, [FromQuery] int limit = 20, [FromQuery] bool? isValid = null) 
    {
        Console.WriteLine("LOG: Tax controller GET request");
        List<TaxModel> gottenTaxes = _taxService.GetTaxes(page_nr, limit, isValid);
        Console.WriteLine("LOG: Tax controller returns Taxes: " + gottenTaxes);
        return Ok(gottenTaxes);
    }

    [HttpPost]
    public IActionResult CreateTax([FromBody] TaxModel tax) 
    {
        Console.WriteLine("CreateTax controller");
        if(!ModelState.IsValid) 
        {
            Console.WriteLine("CreateTax invalid model");
            return BadRequest();
        } else {
            TaxModel? createdTax = _taxService.CreateTax(tax);
            if (createdTax != null){
                return Ok(createdTax);
            } else {
                return StatusCode(501); // http code to be changed(?)
            }
        } 
    }

    [HttpGet("{taxId}")]
    public IActionResult GetTaxById(int taxId) 
    {
        Console.WriteLine("LOG: Tax controller GET GetTaxById request");
        TaxModel? gottenTax = _taxService.GetTax(taxId);
        Console.WriteLine("LOG: Tax controller returns a tax: " + gottenTax);
        if(gottenTax != null)
            return Ok(gottenTax);
        else
            return StatusCode(500, "An error occured while getting a tax"); // to be changed
    }

    [HttpPatch("{taxId}")]
    public IActionResult UpdateTax(int taxId, [FromBody] TaxModel updatedTax)
    {
        Console.WriteLine("LOG: Tax controller DELETE UpdateTax request");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("UpdateTax invalid model");
            return BadRequest("Invalid data provided.");
        }

        var result = _taxService.UpdateTax(taxId, updatedTax);
        if (result == null)
        {
            return NotFound($"Tax with Id {taxId} not found.");
        }
        
        return Ok(result);

    }
}