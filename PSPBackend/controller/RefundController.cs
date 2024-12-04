using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class RefundController : ControllerBase
{
    private readonly RefundService _refundService;

    public RefundController(RefundService refundService)
    {
        _refundService = refundService;
    }

    [HttpGet]
    public IActionResult GetRefunds([FromQuery] RefundGetDto refundGetDto)
    {
        List<RefundModel> gottenRefunds = _refundService.GetRefunds(refundGetDto);
        return Ok(gottenRefunds);
    }

    [HttpGet("{id}")]
    public IActionResult GetRefundById([FromRoute] int id)
    {
        Console.WriteLine("Refund controller GetRefundById with id: " + id);
        var gottenRefund = _refundService.GetRefundById(id);
        return Ok(gottenRefund);
    }

    [HttpPost]
    public IActionResult CreateRefund([FromBody] RefundCreateDto newRefund)
    {
        var createdRefund = _refundService.CreateRefund(newRefund);
        return Ok(createdRefund);
    }
}