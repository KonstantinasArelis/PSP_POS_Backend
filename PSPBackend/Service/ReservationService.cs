using System.Linq;
using PSPBackend.Model;
using System.Text.RegularExpressions;

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

        public ReservationModel CreateReservation(ReservationCreateDto reservationDto)
        {
            int maxReservationDuration = 60*24;
            DateTime threeYearsFromNow = DateTime.Now.AddYears(3);
            DateTime maxReservationDate = threeYearsFromNow;

            if(string.IsNullOrEmpty(reservationDto.client_name))
            {
                Console.WriteLine("Reservation client name is null or empty.");
                throw new ValidationException("Reservation client name is null or empty.");
            }

            if(reservationDto.client_name.Length > 255)
            {
                Console.WriteLine("Reservation client name is too long.");
                throw new ValidationException("Reservation client name is too long.");
            }

            if(string.IsNullOrEmpty(reservationDto.client_phone))
            {
                Console.WriteLine("Reservation client phone is null or empty.");
                throw new ValidationException("Reservation client phone is null or empty.");
            }
            
            string phonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"; 
            if (!Regex.IsMatch(reservationDto.client_phone, phonePattern))
            {
                Console.WriteLine("Invalid phone number format.");
                throw new ValidationException("Invalid phone number format.");
            }

            if(reservationDto.client_phone.Length > 255)
            {
                Console.WriteLine("Reservation client phone number is too long.");
                throw new ValidationException("Reservation client phone number is too long.");
            }

            if(string.IsNullOrEmpty(reservationDto.duration))
            {
                Console.WriteLine("Reservation duration is null or empty.");
                throw new ValidationException("Reservation duration is null or empty.");
            }

            if(reservationDto.duration > maxReservationDuration)
            {
                Console.WriteLine("Reservation duration cannot exceed " + maxReservationDuration);
                throw new ValidationException("Reservation duration cannot exceed " + maxReservationDuration);
            }

            if(reservationDto.duration <= 0)
            {
                Console.WriteLine("Reservation duration cannot be zero or negative.");
                throw new ValidationException("Reservation duration cannot be zero or negative.");
            }
            
            if (reservationDto.appointment_time < DateTime.Now)
            {
                Console.WriteLine("Appointment time cannot be in the past.");
                throw new ValidationException("Appointment time cannot be in the past.");
            }

            if (reservationDto.appointment_time > maxReservationDate)
            {
                Console.WriteLine("Appointment date exceed max availible date.");
                throw new ValidationException("Appointment date exceed max availible date.");
            }

            //Implement once menu managment is added
            if (reservationDto.service_id != null)
            {
                Console.WriteLine("Service id is not used now, supposed to be null");
                throw new ValidationException("Service id is not used now, supposed to be null.");
            }

            ReservationModel newReservation = new ReservationModel();

            newReservation.id = _reservationRepository.GetNewOrderId();

            newReservation.client_name = reservationDto.client_name;
            newReservation.client_phone = reservationDto.client_phone;
            newReservation.appointment_time = reservationDto.appointment_time;
            newReservation.duration = reservationDto.duration;
            newReservation.service_id = reservationDto.service_id;

            //remove later
            newReservation.business_id=null;
            newReservation.employee_id=null;
            newReservation.service_id=null;
            //newReservation.ReservationStatus=null;
            
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