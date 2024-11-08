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
        [FromQuery] int page_nr =0, [FromQuery] int limit = 20, [FromQuery] int? id = null, [FromQuery] int? business_id = null, [FromQuery] int? employee_id = null, 
        [FromQuery] string? client_name = null,[FromQuery] string?  client_phone = null, [FromQuery] DateTime? created_before = null, [FromQuery] DateTime? created_after = null, 
        [FromQuery] DateTime? last_modified_before = null, [FromQuery] DateTime? last_modified_after = null,
        [FromQuery] DateTime? appointment_time_before = null, [FromQuery] DateTime? appointment_time_after = null, 
        [FromQuery] int? duration_less_than = null, [FromQuery] int? duration_more_than = null, [FromQuery] int? status = null, [FromQuery] int? service_id = null
        ) 
    {

        //separate this into validator later
        if(limit>100)
        {
            return BadRequest();
        }
        Console.WriteLine("LOG: Reservation controller GET request");
        List<ReservationModel> gottenReservation = _reservationService.GetReservation(
                page_nr, limit, id, business_id, employee_id, 
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
                return StatusCode(501); // http code to be changed
            }
        }
        
        
    }
}