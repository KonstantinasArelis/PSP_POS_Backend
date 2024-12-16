using System.Linq;
using PSPBackend.Model;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
            var query = _reservationRepository.GetReservations(reservationGetDto); 

            var reservation = query.Skip(reservationGetDto.page_nr * reservationGetDto.limit).Take(reservationGetDto.limit).ToList();
            return reservation;
        }

        public ReservationModel GetReservationById(int id)
        {
            ReservationModel gottenReservation;
            try {
                gottenReservation = _reservationRepository.GetReservationById(id);
            } catch (KeyNotFoundException ex) {
                throw;
            }
            return gottenReservation;
        }

        public ReservationModel CreateReservation(ReservationCreateDto reservationDto)
        {
            // map dto to a model for database
            ReservationModel newReservation = new ReservationModel();
            newReservation.id = _reservationRepository.GetNewOrderId();
            newReservation.client_name = reservationDto.client_name;
            newReservation.client_phone = reservationDto.client_phone;
            newReservation.appointment_time = reservationDto.appointment_time;
            newReservation.duration = reservationDto.duration;
            newReservation.service_id = reservationDto.service_id;

            // Set some properties
            newReservation.created_at = DateTime.Now;
            newReservation.last_modified = DateTime.Now;
            newReservation.ReservationStatus = reservationStatusEnum.RESERVED;

            newReservation.business_id= reservationDto.business_id;
            newReservation.employee_id = reservationDto.employee_id;

            // TO-DO change once menu managemet is implemented
            newReservation.service_id=null;
            
            try {
                _reservationRepository.CreateReservation(newReservation);
            } catch (DbUpdateException ex) {
                throw;
            }
            
            try {
                // create order for reservation
                OrderModel newOrder = new OrderModel();
                newOrder.id = 0; // set to 0 because then CreateOrder finds a new id
                newOrder.employee_id = "3AFF0D9B-5399-4D39-8B15-F3C158F8359F"; // set to 1 since authorization is not implemented yet

                _orderService.CreateOrder(newOrder);

                // create order item for reservation (reservations are tied to order items)
                OrderItemModel newOrderItem = new OrderItemModel();
                newOrderItem.id = 0; // set to 0 because then AddItem finds a new id
                newOrderItem.quantity = 1;

                newOrderItem.reservation_id = newReservation.id;

                // product_id, product_name, product_price, tax_id, item_discount_amount not set as they are not implemented yet
                _orderService.AddItem(newOrder.id, newOrderItem);
            } catch (DbUpdateException ex) {
                throw;
            }
            
            return newReservation;
        }

        public ReservationModel UpdateReservation(int id, ReservationPatchDto reservationDto)
        {
            try{
                ReservationModel result = _reservationRepository.UpdateReservation(id, reservationDto);

                if(reservationDto.ReservationStatus == reservationStatusEnum.CANCELLED)
                {
                    int orderId = _orderService.getOrderItemByReservationId(id).order_id;
                    _orderService.DeleteOrder(orderId);
                }
                return result;
            } catch (DbUpdateException ex){
                throw;
            } catch (KeyNotFoundException ex){
                throw;
            }
        }
}