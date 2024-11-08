using System.Linq;
using PSPBackend.Model;
public class ReservationService
{
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;  
        }

        public List<ReservationModel> GetReservation(
            int page_nr, int limit, int? id, int? business_id , int? employee_id , 
            string? client_name, string? client_phone, DateTime? created_before, DateTime? created_after, 
            DateTime? last_modified_before, DateTime? last_modified_after,
            DateTime? appointment_time_before, DateTime? appointment_time_after, 
            int? duration_less_than, int? duration_more_than, int? status, int? service_id
        )
        {
            
            Console.WriteLine("LOG: Reservation service GetReservation");
            var query = _reservationRepository.GetReservation(
                page_nr, limit, id, business_id, employee_id, 
                client_name,client_phone, created_before, created_after, 
                last_modified_before, last_modified_after,
                appointment_time_before, appointment_time_after, 
                duration_less_than, duration_more_than, status, service_id
            ); 

            var reservation = query.Skip(page_nr * limit).Take(limit).ToList();
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