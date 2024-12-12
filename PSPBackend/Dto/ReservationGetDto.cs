using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ReservationGetDto
{
    [Range(0, int.MaxValue, ErrorMessage = "Page number must be a non-negative integer.")]
    public int page_nr { get; set; } = 0;

    [Range(0, 100, ErrorMessage = "Page limit must be between 0 and 100")]
    public int limit { get; set; } = 20;

    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")] 
    public int? id { get; set; }

    //user authorization is not implemented, so this cant be validated
    //[Range(1, int.MaxValue, ErrorMessage = "Business Id must be a positive integer.")] 
    public int? business_id { get; set; }

    //user authorization is not implemented, so this cant be validated
    //[Range(1, int.MaxValue, ErrorMessage = "Employee Id must be a positive integer.")] 
    public int? employee_id { get; set; }

    [ClientNameAttibute]
    public string? client_name { get; set; }

    [Phone] 
    public string? client_phone { get; set; }

    public DateTime? created_before { get; set; }
    public DateTime? created_after { get; set; }
    public DateTime? last_modified_before { get; set; }
    public DateTime? last_modified_after { get; set; }
    public DateTime? appointment_time_before { get; set; }
    public DateTime? appointment_time_after { get; set; }

    [ReservationDuration]
    public int? duration_less_than { get; set; }

    [ReservationDuration]
    public int? duration_more_than { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public reservationStatusEnum? status { get; set; }

    //menu managment is not implemented, so this cant be validated
    //[Range(1, int.MaxValue, ErrorMessage = "Service Id must be a positive integer.")] 
    public int? service_id { get; set; }
}