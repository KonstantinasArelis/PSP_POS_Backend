using System.Linq;
using PSPBackend.Model;
public class ReservationService
{
        private readonly ReservationRepository _reservationRepository;
        private readonly OrderService _orderService;

        public ReservationService(ReservationRepository reservationRepository, OrderService orderService)
        {
            _reservationRepository = reservationRepository;
            _orderService = orderService;
        }

        public List<ReservationModel> GetReservations(ReservationGetDto reservationGetDto)
        {
            Console.WriteLine("LOG: Reservation service GetReservation");
            var query = _reservationRepository.GetReservations(reservationGetDto); 

            var reservation = query.Skip(reservationGetDto.page_nr * reservationGetDto.limit).Take(reservationGetDto.limit).ToList();
            return reservation;
        }

        public ReservationModel GetReservationById(int id)
        {
            ReservationModel gottenReservation = _reservationRepository.GetReservationById(id);
            return gottenReservation;
        }

        public ReservationModel CreateReservation(ReservationCreateDto reservation)
        {
            Console.WriteLine("CreateReservation service");
            ReservationModel newReservation = new ReservationModel();

            newReservation.id = _reservationRepository.GetNewOrderId();
            newReservation.client_name = reservation.client_name;
            newReservation.client_phone = reservation.client_phone;
            newReservation.appointment_time = reservation.appointment_time;
            newReservation.duration = reservation.duration;
            newReservation.service_id = reservation.service_id;

            //remove later
            newReservation.business_id=null;
            newReservation.employee_id=null;
            newReservation.service_id=null;
            newReservation.ReservationStatus=null;
            
            if (_reservationRepository.CreateReservation(newReservation) > 0){
                OrderModel newOrder = new OrderModel();
                newOrder.id = 0;
                _orderService.CreateOrder(newOrder);

                OrderItemModel newOrderItem = new OrderItemModel();
                newOrderItem.reservation_id = newReservation.id;
                _orderService.AddItem(newOrder.id, newOrderItem);

                return newReservation;
            } else{
                return null;
            }
        }

        public int UpdateReservation(int id, ReservationPatchDto reservationDto)
        {
            int result = _reservationRepository.UpdateReservation(id, reservationDto);
            return result;
        }
}