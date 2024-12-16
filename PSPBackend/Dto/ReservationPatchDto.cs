using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ReservationPatchDto // TO-DO API documentation says  created_at, last_modified properies sohuld be part of this dto, but that doesnt make sense so they are not here.
{
        //user authorization is not implemented, so this cant be validated
        //[Range(1, int.MaxValue, ErrorMessage = "Business Id must be a positive integer.")] 
        public int? business_id { get; set; }

        //user authorization is not implemented, so this cant be validated
        //[Range(1, int.MaxValue, ErrorMessage = "Employee Id must be a positive integer.")] 
        public string? employee_id { get; set; }

        [ClientNameAttibute]
        public string? client_name { get; set; }

        [Phone]
        public string? client_phone { get; set; }

        [FutureDate]
        public DateTime? appointment_time { get; set; }

        [ReservationDuration]
        public int? duration { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public reservationStatusEnum? ReservationStatus { get; set; }

        //menu managment is not implemented, so this cant be validated
        //[Range(1, int.MaxValue, ErrorMessage = "Service Id must be a positive integer.")] 
        public int? service_id { get; set; }
}