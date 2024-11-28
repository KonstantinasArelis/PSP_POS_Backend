using System.Linq;
using PSPBackend.Model;
public class ReservationService
{
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;  
        }

        public List<ReservationModel> GetReservation(ReservationGetDto reservationGetDto)
        {
            Console.WriteLine("LOG: Reservation service GetReservation");
            var query = _reservationRepository.GetReservation(reservationGetDto); 

            var reservation = query.Skip(reservationGetDto.page_nr * reservationGetDto.limit).Take(reservationGetDto.limit).ToList();
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