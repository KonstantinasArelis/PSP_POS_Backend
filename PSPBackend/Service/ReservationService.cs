using PSPBackend.Model;
public class ReservationService
{
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;  
        }

        public List<ReservationModel> GetReservation(
            int page_nr, int limit, int employee_id, 
            int client_name, int  client_phone, DateTime created_before, DateTime created_after, 
            DateTime last_modified_before, DateTime last_modified_after,
            DateTime appointment_time_before, DateTime appointment_time_after, 
            int duration_less_than, int duration_more_than, int status, int service_id
        )
        {
            Console.WriteLine("LOG: Reservation service GetReservation");
            var query = _reservationRepository.GetReservation(
                page_nr, limit, employee_id, 
                client_name,client_phone, created_before, created_after, 
                last_modified_before, last_modified_after,
                appointment_time_before, appointment_time_after, 
                duration_less_than, duration_more_than, status, service_id
            ); 
            var pageSize = 20;
            var reservation = query.Skip(page_nr * pageSize).Take(pageSize).ToList();
            return reservation;
        }

        public ReservationModel CreateReservation(ReservationModel reservation)
        {
            Console.WriteLine("CreateReservation service");

            //remove later
            reservation.business_id=null;
            reservation.employee_id=null;
            reservation.service_id=null;
            reservation.ReservationStatus=null;
            
            if (_reservationRepository.CreateReservation(reservation) > 0){
                return reservation;
            } else{
                return null;
            }
        }
}