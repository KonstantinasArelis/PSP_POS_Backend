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
    public IActionResult Get([FromQuery]ReservationGetDto reservationGetDto) 
    {        
        //separate this into validator later
        if(reservationGetDto.limit>100)
        {
            return BadRequest();
        }
        Console.WriteLine("LOG: Reservation controller GET request");
        List<ReservationModel> gottenReservation = _reservationService.GetReservation(reservationGetDto);
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