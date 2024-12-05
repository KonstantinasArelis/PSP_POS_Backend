using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        List<BusinessModel> gottenBusinesses = _businessService.getBusinesses(businessGetDto);

        return Ok(gottenBusinesses);
    }

    [HttpGet("{id}")]
    public IActionResult getBusinessById ([FromRoute] int id)
    {
        Console.WriteLine("Business controller getBusinessById with id: " + id);
        var gottenBusiness = _businessService.getBusinessById(id);

        return Ok(gottenBusiness);
    }

    [HttpPost]
    public IActionResult createBusiness ([FromBody] BusinessCreateDto newBusiness)
    {
        var createdBusiness = _businessService.createBusiness(newBusiness);

        return Ok(createdBusiness);
    }

    [HttpPatch("{id}")]
    public IActionResult updateBusiness ([FromRoute] int id, [FromBody] BusinessUpdateDto updatedBusinessDto)
    {
        var updatedBusiness = _businessService.updateBusiness(id, updatedBusinessDto);

        return Ok(updatedBusiness);
    }

    [HttpDelete("{id}")]
    public IActionResult deleteBusiness ([FromRoute] int businessId)
    {
        var result = _businessService.deleteBusiness(businessId);

        return Ok(result);
    }
}