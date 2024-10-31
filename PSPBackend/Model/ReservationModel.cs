namespace PSPBackend.Model
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int Duration { get; set; }
        public int ReservationStatus { get; set; }
        public int ServiceId { get; set; }
    }
}
