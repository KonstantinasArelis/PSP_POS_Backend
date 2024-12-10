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

    /*
    [HttpGet]
    public string Get()
    {
        Console.WriteLine("LOG: controller creates order");
        string temp = _orderService.CreateOrder();
        Console.WriteLine("LOG: controller supposed to return: " + temp);
        return temp;
    }
    */
    [HttpGet]
    public IActionResult GetReservations([FromQuery]ReservationGetDto reservationGetDto) 
    {        
        //separate this into validator later
        if(reservationGetDto.limit>100)
        {
            return BadRequest();
        }
        Console.WriteLine("LOG: Reservation controller GET request");
        List<ReservationModel> gottenReservation = _reservationService.GetReservations(reservationGetDto);
        Console.WriteLine("LOG: Reservation controller returns orders: " + gottenReservation);
        return Ok(gottenReservation);
        //return NotFound();
    }

    [HttpGet("{id}")]
    public IActionResult GetReservationById(int id)
    {
        Console.WriteLine("TESTTTTTTTTTTTTT");
        ReservationModel gottenReservation = _reservationService.GetReservationById(id);
        return Ok(gottenReservation);
    }



    [HttpPost]
    public IActionResult CreateReservation([FromBody] ReservationCreateDto reservation)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest("Invalid Dto");
        }

        ReservationModel result;
        try{
            result = _reservationService.CreateReservation(reservation);
        } catch(ValidationException ex) {
            return BadRequest (new {error = ex.Message});
        } catch(Exception ex) {
            return StatusCode(500, new {error = ex.Message});
        }

        return Ok();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateReservation(int id, [FromBody] ReservationPatchDto reservationDto)
    {
        int result = _reservationService.UpdateReservation(id, reservationDto);

        if(result == 1){
            return Ok();
        }
        else if(result == 0){
            return NotFound();
        } else {
            return StatusCode(501); // http code to be changed
        }
    }
}