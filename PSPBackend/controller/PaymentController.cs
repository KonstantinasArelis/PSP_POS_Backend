using System.ComponentModel.DataAnnotations;
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
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<PaymentModel> gottenPayments = _paymentService.GetPayments(paymentGetDto);
        return Ok(gottenPayments);
    }

    [HttpGet("{id}")]
    public IActionResult GetPaymentById([FromRoute] int id)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        PaymentModel gottenPayment;

        try {
            _paymentService.GetPaymentById(id);
        } catch (KeyNotFoundException ex) {
            return NotFound();
        }

        return Ok(gottenPayment);
    }

    [HttpPost]
    public IActionResult CreatePayment([FromBody] PaymentCreateDto newPayment)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        PaymentModel result;
        try 
        {
            result = _paymentService.CreatePayment(newPayment);
        } catch (ValidationException ex) {
            return BadRequest(new {error = ex.Message});
        } catch(DbUpdateException ex) {
            return StatusCode(500);
        }

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePayment([FromRoute] int paymentId, [FromBody] PaymentUpdateDto updatedPaymentDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        PaymentModel updatedPayment;

        try {
            updatedPayment = _paymentService.UpdatePayment(paymentId, updatedPaymentDto);
        } catch (ValidationException ex) {
            return BadRequest(new {error = ex.Message});
        } catch(DbUpdateException ex) {
            return StatusCode(500);
        }

        return Ok(updatedPayment);
    }
}