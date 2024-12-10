namespace PSPBackend.Model
{
    public class ReservationCreateDto
    {
        public string client_name { get; set; }
        public string client_phone { get; set; }
        public DateTime? appointment_time { get; set; }
        public int duration { get; set; } // TO-DO api contract says this is not supposed to be here
        public int? service_id { get; set; }
    }
}
