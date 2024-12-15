using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class BusinessController : ControllerBase
{
    private readonly BusinessService _businessService;
    private readonly ILogger<BusinessController> _logger;

    public BusinessController (BusinessService businessService, ILogger<BusinessController> logger) 
    {
        _businessService = businessService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult getBusinesses ([FromQuery] BusinessGetDto businessGetDto) 
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("BusinessController encountered a problem in GetBusinesses (returning status 400)");
            return BadRequest(ModelState);
        }

        List<BusinessModel> gottenBusinesses = _businessService.getBusinesses(businessGetDto);

        _logger.LogInformation("BusinessController successfully executed GetBusinesses");
        return Ok(gottenBusinesses);
    }

    [HttpGet("{id}")]
    public IActionResult getBusinessById ([FromRoute] int id)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("BusinessController encountered a problem in GetBusinessById (returning status 400)");
            return BadRequest(ModelState);
        }

        BusinessModel result;
        try {
           result = _businessService.getBusinessById(id);
        } catch (KeyNotFoundException e){
            _logger.LogError("BusinessController encountered a problem in GetBusinessById (returning status 404) | " + e.Message);
            return NotFound();
        }
         
        _logger.LogInformation("BusinessController successfully executed GetBusinessById");
        return Ok(result);
    }

    [HttpPost]
    public IActionResult createBusiness ([FromBody] BusinessCreateDto newBusiness)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("BusinessController encountered a problem in CreateBusiness (returning status 400)");
            return BadRequest(ModelState);
        }

        BusinessModel createdBusiness;
        
        try {
            createdBusiness = _businessService.createBusiness(newBusiness);
        } catch (DbUpdateException e){
            _logger.LogError("BusinessController encountered a problem in CreateBusiness (returning status 500) | " + e.Message);
            return StatusCode(500);
        }
        
        _logger.LogInformation("BusinessController successfully executed CreateBusiness");
        return Ok(createdBusiness);
    }

    [HttpPatch("{id}")]
    public IActionResult updateBusiness ([FromRoute] int id, [FromBody] BusinessUpdateDto updatedBusinessDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("BusinessController encountered a problem in UpdateBusiness (returning status 400)");
            return BadRequest(ModelState);
        }


        BusinessModel updatedBusiness;

        try{
            updatedBusiness = _businessService.updateBusiness(id, updatedBusinessDto);
        } catch (DbUpdateException e){
            _logger.LogError("BusinessController encountered a problem in UpdateBusiness (returning status 500) | " + e.Message);
            return StatusCode(500);
        } catch (KeyNotFoundException e){
            _logger.LogError("BusinessController encountered a problem in UpdateBusiness (returning status 404) | " + e.Message);
            return NotFound();
        }

        _logger.LogInformation("BusinessController successfully executed UpdateBusiness");
        return Ok(updatedBusiness);
    }

    [HttpDelete("{id}")]
    public IActionResult deleteBusiness ([FromRoute] int businessId)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("BusinessController encountered a problem in DeleteBusiness (returning status 400)");
            return BadRequest(ModelState);
        }

        try{
            _businessService.deleteBusiness(businessId);
        } catch(DbUpdateException e){
            _logger.LogError("BusinessController encountered a problem in DeleteBusiness (returning status 500) | " + e.Message);
            return StatusCode(500);
        }

        _logger.LogInformation("BusinessController successfully executed DeleteBusiness");
        return Ok();
    }
}