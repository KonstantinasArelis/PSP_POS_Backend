using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public IActionResult GetPayments([FromQuery] PaymentGetDto paymentGetDto)
    {
        List<PaymentModel> gottenPayments = _paymentService.GetPayments(paymentGetDto);
        return Ok(gottenPayments);
    }

    [HttpGet("{id}")]
    public IActionResult GetPaymentById([FromRoute] int id)
    {
        Console.WriteLine("Payment controller GetPaymentById with id: " + id);
        var gottenPayment = _paymentService.GetPaymentById(id);
        return Ok(gottenPayment);
    }

    [HttpPost]
    public IActionResult CreatePayment([FromBody] PaymentCreateDto newPayment)
    {
        var createdPayment = _paymentService.CreatePayment(newPayment);
        return Ok(createdPayment);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePayment([FromRoute] int paymentId, [FromBody] PaymentUpdateDto updatedPaymentDto)
    {
        var updatedPayment = _paymentService.UpdatePayment(paymentId, updatedPaymentDto);
        return Ok(updatedPayment);
    }
}