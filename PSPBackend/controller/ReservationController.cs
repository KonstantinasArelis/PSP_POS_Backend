using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;
    
    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
        
    }

    [HttpGet]
    public IActionResult GetReservations([FromQuery]ReservationGetDto reservationGetDto) 
    {   
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        List<ReservationModel> result;

        try {
            result = _reservationService.GetReservations(reservationGetDto);
        } catch (ValidationException ex) {
            return BadRequest( new {error = ex.Message});
        } catch (Exception ex) {
            return StatusCode(500);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetReservationById(int id)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ReservationModel gottenReservation;
        try {
            gottenReservation = _reservationService.GetReservationById(id);
        } catch (KeyNotFoundException ex) {
            return NotFound();
        }
        
        return Ok(gottenReservation);
    }



    [HttpPost]
    public IActionResult CreateReservation([FromBody] ReservationCreateDto reservation)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ReservationModel result;
        try{
            result = _reservationService.CreateReservation(reservation);
        } catch(ValidationException ex) {
            return BadRequest (new {error = ex.Message});
        } catch(Exception ex) {
            return StatusCode(500);
        }

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateReservation(int id, [FromBody] ReservationPatchDto reservationDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ReservationModel result;
        try {
            result = _reservationService.UpdateReservation(id, reservationDto);
        } catch(DbUpdateException ex) {
            return StatusCode(500);
        } catch(KeyNotFound ex) {
            return NotFound();
        }

        return Ok(result);
    }
}