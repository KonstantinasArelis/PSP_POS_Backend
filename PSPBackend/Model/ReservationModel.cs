using System.Text.Json.Serialization;

namespace PSPBackend.Model
{
    public class ReservationModel
    {
        public int id { get; set; }
        public int? business_id { get; set; }
        public string? employee_id { get; set; }
        public string? client_name { get; set; }
        public string? client_phone { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? last_modified { get; set; }
        public DateTime? appointment_time { get; set; }
        public int? duration { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public reservationStatusEnum? ReservationStatus { get; set; }
        public string? service_id { get; set; }
    }
}
