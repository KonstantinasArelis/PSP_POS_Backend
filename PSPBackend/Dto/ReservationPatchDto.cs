public class ReservationPatchDto
{
    public int id { get; set; }
    public int? business_id { get; set; }
    public int? employee_id { get; set; }
    public string? client_name { get; set; }
    public string? client_phone { get; set; }
    public DateTime? created_before { get; set; }
    public DateTime? created_after { get; set; }
    public DateTime? last_modified_before { get; set; }
    public DateTime? last_modified_after { get; set; }
    public DateTime? appointment_time_before { get; set; }
    public DateTime? appointment_time_after { get; set; }
    public int? duration_less_than { get; set; }
    public int? duration_more_than { get; set; }
    public int? status { get; set; }
    public int? service_id { get; set; }
}