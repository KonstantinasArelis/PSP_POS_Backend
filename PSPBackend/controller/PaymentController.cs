using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(PaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetPayments([FromQuery] PaymentGetDto paymentGetDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("PaymentController encountered a problem in GetPayments (returning status 400)");
            return BadRequest(ModelState);
        }

        List<PaymentModel> gottenPayments = _paymentService.GetPayments(paymentGetDto);
        _logger.LogInformation("PaymentController successfully executed GetPayments");
        return Ok(gottenPayments);
    }

    [HttpGet("{id}")]
    public IActionResult GetPaymentById([FromRoute] int id)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("PaymentController encountered a problem in GetPaymentById (returning status 400)");
            return BadRequest(ModelState);
        }

        PaymentModel result;

        try {
            result = _paymentService.GetPaymentById(id);
        } catch (KeyNotFoundException) {
            _logger.LogError("PaymentController encountered a problem in GetPaymentById (returning status 404)");
            return NotFound();
        }
        _logger.LogInformation("PaymentController successfully executed GetPaymentById");
        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreatePayment([FromBody] PaymentCreateDto newPayment)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("PaymentController encountered a problem in CreatePayment (returning status 400)");
            return BadRequest(ModelState);
        }

        PaymentModel result;
        try 
        {
            result = _paymentService.CreatePayment(newPayment);
        } catch (ValidationException e) {
            _logger.LogError("PaymentController encountered a problem in CreatePayment (returning status 400) | " + e.Message);
            return BadRequest(new {error = e.Message});
        } catch(DbUpdateException e) {
            _logger.LogError("PaymentController encountered a problem in CreatePayment (returning status 500) | " + e.Message);
            return StatusCode(500);
        }
        _logger.LogInformation("PaymentController successfully executed CreatePayment");
        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePayment([FromRoute] int paymentId, [FromBody] PaymentUpdateDto updatedPaymentDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("PaymentController encountered a problem in UpdatePayment (returning status 400)");
            return BadRequest(ModelState);
        }

        PaymentModel updatedPayment;

        try {
            updatedPayment = _paymentService.UpdatePayment(paymentId, updatedPaymentDto);
        } catch (ValidationException ex) {
            _logger.LogError("PaymentController encountered a problem in UpdatePayment (returning status 400)");
            return BadRequest(new {error = ex.Message});
        } catch(DbUpdateException e) {
            _logger.LogError("PaymentController encountered a problem in UpdatePayment (returning status 500) | " + e.Message);
            return StatusCode(500);
        } catch(KeyNotFoundException e) {
            _logger.LogError("PaymentController encountered a problem in UpdatePayment (returning status 404) | " + e.Message);
            return NotFound();
        }
        _logger.LogInformation("PaymentController successfully executed UpdatePayment");
        return Ok(updatedPayment);
    }
}