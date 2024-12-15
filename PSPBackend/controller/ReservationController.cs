using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;
    
    public ReservationController(ReservationService reservationService, ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetReservations([FromQuery]ReservationGetDto reservationGetDto) 
    {   
        if(!ModelState.IsValid)
        {
            _logger.LogError("ReservationController encountered a problem in GetReservations (returning status 400)");
            return BadRequest(ModelState);
        }

        List<ReservationModel> result;

        try {
            result = _reservationService.GetReservations(reservationGetDto);
        } catch (ValidationException e) {
            _logger.LogError("ReservationController encountered a problem in GetReservations (returning status 400) | " + e.Message);
            return BadRequest( new {error = e.Message});
        }
        _logger.LogInformation("ReservationController successfully executed GetReservations");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetReservationById(int id)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("ReservationController encountered a problem in GetReservationById (returning status 400)");
            return BadRequest(ModelState);
        }

        ReservationModel gottenReservation;
        try {
            gottenReservation = _reservationService.GetReservationById(id);
        } catch (KeyNotFoundException e) {
            _logger.LogError("ReservationController encountered a problem in GetReservationById (returning status 400) | " + e.Message);
            return NotFound();
        }
        _logger.LogInformation("ReservationController successfully executed GetReservationById");
        return Ok(gottenReservation);
    }



    [HttpPost]
    public IActionResult CreateReservation([FromBody] ReservationCreateDto reservation)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("ReservationController encountered a problem in CreateReservations (returning status 400)");
            return BadRequest(ModelState);
        }

        ReservationModel result;
        try{
            result = _reservationService.CreateReservation(reservation);
        } catch(ValidationException e) {
            _logger.LogError("ReservationController encountered a problem in CreateReservation (returning status 400)");
            return BadRequest (new {error = e.Message});
        } catch(DbUpdateException e) {
            _logger.LogError("ReservationController encountered a problem in CreateReservation (returning status 500) | " + e.Message);
            return StatusCode(500);
        }
        _logger.LogInformation("ReservationController successfully executed CreateReservation");
        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateReservation(int id, [FromBody] ReservationPatchDto reservationDto)
    {
        if(!ModelState.IsValid)
        {
            _logger.LogError("ReservationController encountered a problem in UpdateReservation (returning status 400)");
            return BadRequest(ModelState);
        }

        ReservationModel result;
        try {
            result = _reservationService.UpdateReservation(id, reservationDto);
        } catch(DbUpdateException e) {
            _logger.LogError("ReservationController encountered a problem in UpdateReservation (returning status 500) | " + e.Message);
            return StatusCode(500);
        } catch(KeyNotFoundException e) {
            _logger.LogError("ReservationController encountered a problem in UpdateReservation (returning status 404) | " + e.Message);
            return NotFound();
        }
        _logger.LogInformation("ReservationController successfully executed UpdateReservation");
        return Ok(result);
    }
}