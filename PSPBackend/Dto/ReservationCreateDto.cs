using System.ComponentModel.DataAnnotations;

namespace PSPBackend.Model
{
    public class ReservationCreateDto
    {
        [ClientNameAttibute]
        public string client_name { get; set; }

        [Phone] 
        public string client_phone { get; set; }

        [FutureDate]
        public DateTime? appointment_time { get; set; }

        [ReservationDuration]
        public int duration { get; set; } // TO-DO api contract says this is not supposed to be here

        //menu managment is not implemented, so this cant be validated
        //[Range(1, int.MaxValue, ErrorMessage = "Service Id must be a positive integer.")] 
        public int? service_id { get; set; }

        public string? employee_id { get; set; }

        public int? business_id { get; set; }
    }
}
