using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class BusinessController : ControllerBase
{
    private readonly BusinessService _businessService;

    public BusinessController (BusinessService businessService) 
    {
        _businessService = businessService;
    }

    [HttpGet]
    public IActionResult getBusinesses ([FromQuery] BusinessGetDto businessGetDto) 
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<BusinessModel> gottenBusinesses = _businessService.getBusinesses(businessGetDto);

        return Ok(gottenBusinesses);
    }

    [HttpGet("{id}")]
    public IActionResult getBusinessById ([FromRoute] int id)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        BusinessModel result;
        try {
           result = _businessService.getBusinessById(id);
        } catch (KeyNotFoundException){
            return NotFound();
        }
         

        return Ok(result);
    }

    [HttpPost]
    public IActionResult createBusiness ([FromBody] BusinessCreateDto newBusiness)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        BusinessModel createdBusiness;
        
        try {
            createdBusiness = _businessService.createBusiness(newBusiness);
        } catch (DbUpdateException){
            return StatusCode(500);
        }
        
        return Ok(createdBusiness);
    }

    [HttpPatch("{id}")]
    public IActionResult updateBusiness ([FromRoute] int id, [FromBody] BusinessUpdateDto updatedBusinessDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        BusinessModel updatedBusiness;

        try{
            updatedBusiness = _businessService.updateBusiness(id, updatedBusinessDto);
        } catch (DbUpdateException){
            return StatusCode(500);
        } catch (KeyNotFoundException){
            return NotFound();
        }

        return Ok(updatedBusiness);
    }

    [HttpDelete("{id}")]
    public IActionResult deleteBusiness ([FromRoute] int businessId)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try{
            _businessService.deleteBusiness(businessId);
        } catch(DbUpdateException){
            return StatusCode(500);
        }

        return Ok();
    }
}