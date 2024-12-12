using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        } catch (KeyNotFoundException) {
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
        } catch(DbUpdateException) {
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
        } catch(DbUpdateException) {
            return StatusCode(500);
        } catch(KeyNotFoundException) {
            return NotFound();
        }

        return Ok(result);
    }
}