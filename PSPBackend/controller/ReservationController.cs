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
    public IActionResult Get(
        [FromQuery] int page_nr, [FromQuery] int limit, [FromQuery] int employee_id, 
        [FromQuery] int client_name,[FromQuery] int  client_phone, [FromQuery] DateTime created_before, [FromQuery] DateTime created_after, 
        [FromQuery] DateTime last_modified_before, [FromQuery] DateTime last_modified_after,
        [FromQuery] DateTime appointment_time_before, [FromQuery] DateTime appointment_time_after, 
        [FromQuery] int duration_less_than, [FromQuery] int duration_more_than, [FromQuery] int status, [FromQuery] int service_id
        ) 
    {
        Console.WriteLine("LOG: Reservation controller GET request");
        List<ReservationModel> gottenReservation = _reservationService.GetReservation(
            page_nr, limit, employee_id, 
            client_name,client_phone, created_before, created_after, 
            last_modified_before, last_modified_after,
            appointment_time_before, appointment_time_after, 
            duration_less_than, duration_more_than, status, service_id
            );
        Console.WriteLine("LOG: Reservation controller returns orders: " + gottenReservation);
        return Ok(gottenReservation);
        //return NotFound();
    }

    [HttpPost]
    public IActionResult CreateReservation([FromBody] ReservationModel reservation)
    {
        Console.WriteLine("CreateReservation controller");
        if(!ModelState.IsValid)
        {
            Console.WriteLine("CreateReservation invalid model");
            return BadRequest();
        } else {
            if (_reservationService.CreateReservation(reservation) != null){
                return Ok();
            } else {
                return StatusCode(501); // code to be changed
            }
        }
        
        
    }
}