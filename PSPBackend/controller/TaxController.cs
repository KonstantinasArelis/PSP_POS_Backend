using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class TaxController : ControllerBase
{
    private readonly TaxService _taxService;
    private readonly ILogger<TaxController> _logger;

    public TaxController(TaxService taxService, ILogger<TaxController> logger)
    {
        _taxService = taxService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetTaxes([FromQuery] int page_nr = 0, [FromQuery] int limit = 20, [FromQuery] bool? isValid = null) 
    {
        List<TaxModel> gottenTaxes = _taxService.GetTaxes(page_nr, limit, isValid);
        _logger.LogInformation("TaxController successfully executed GetTaxes");
        return Ok(gottenTaxes);
    }

    [HttpPost]
    public IActionResult CreateTax([FromBody] TaxModel tax) 
    {
        if(!ModelState.IsValid) 
        {
            _logger.LogError("TaxController encountered a problem in CreateTax (returning status 400)");
            return BadRequest();
        } else {
            TaxModel? createdTax = _taxService.CreateTax(tax);
            if (createdTax != null){
                _logger.LogInformation("TaxController successfully executed CreateTax");
                return Ok(createdTax);
            } else {
                _logger.LogError("TaxController encountered a problem in CreateTax (returning status 501)");
                return StatusCode(501); 
            }
        } 
    }

    [HttpGet("{taxId}")]
    public IActionResult GetTaxById(int taxId) 
    {
        TaxModel? gottenTax = _taxService.GetTax(taxId);
        if(gottenTax != null)
        {
            _logger.LogInformation("TaxController successfully executed GetTaxById");
            return Ok(gottenTax);
        }
        else
        {
            _logger.LogError("TaxController encountered a problem in GetTaxById (returning status 500)");
            return StatusCode(500, "An error occured while getting a tax");
        }
    }

    [HttpPatch("{taxId}")]
    public IActionResult UpdateTax(int taxId, [FromBody] TaxModel updatedTax)
    {        
        if (!ModelState.IsValid)
        {
            _logger.LogError("TaxController encountered a problem in UpdateTax (returning status 400)");
            return BadRequest("Invalid data provided.");
        }

        var result = _taxService.UpdateTax(taxId, updatedTax);
        if (result == null)
        {
            _logger.LogError("TaxController encountered a problem in UpdateTax (returning status 404)");
            return NotFound($"Tax with Id {taxId} not found.");
        }
        _logger.LogInformation("TaxController successfully executed UpdateTax");
        return Ok(result);
    }
}