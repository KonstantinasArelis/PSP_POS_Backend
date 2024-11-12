using PSPBackend.Model;
public class OrderService
{
        private readonly OrderRepository _orderRepository;
        private readonly OrderStatusRepository _orderStatusRepository;
        public OrderService(OrderRepository orderRepository, OrderStatusRepository orderStatusRepository)
        {
            _orderRepository = orderRepository;  
            _orderStatusRepository = orderStatusRepository;
        }

        public int? CreateOrder(OrderModel order)
        {
            Console.WriteLine("LOG: service creates order");
            return _orderRepository.AddOrder(order);
        }

        public List<OrderModel> GetOrders(
            int page_nr = 0, int limit = 20, int? employee_id = null, 
            decimal? min_total_amount = null, decimal? max_total_amount = null, 
            string? order_status = null
        )
        {
            int? int_status = null;
            if(order_status != null)
            {
                Console.WriteLine("|||||||||||||||||||here||||||||||||||||||||||||||||||||||||||");
                int_status = _orderStatusRepository.ConvertNameToCode(order_status);
            }
            Console.WriteLine("LOG: Order service GetOrders");
            //List<OrderModel> orders = new List<OrderModel>();
            var query = _orderRepository.GetOrders(employee_id, min_total_amount, 
                                          max_total_amount, int_status); 
            var pageSize = 20;
            var orders = query.Skip(page_nr * pageSize).Take(pageSize).ToList();
            return orders;
        }

        public OrderModel? GetOrder(int order_id)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            return _orderRepository.GetOrder(order_id);
        }

        public void UpdateOrder(OrderModel order, string? order_status = null)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            if(order_status != null)
            {
                int? order_status_int = _orderStatusRepository.ConvertNameToCode(order_status);
                order.order_status = order_status_int;
            }
            _orderRepository.UpdateOrder(order);            
        }
}