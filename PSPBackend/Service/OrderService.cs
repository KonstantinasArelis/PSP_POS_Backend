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

        public List<OrderModel> GetOrders(OrderArgumentModel arguments)
        {
            int? int_status = null;
            if(arguments.OrderStatus != null)
            {
                int_status = _orderStatusRepository.ConvertNameToCode(arguments.OrderStatus);
            }
            Console.WriteLine("LOG: Order service GetOrders");
            var query = _orderRepository.GetOrders(arguments, int_status);
            var pageSize = arguments.Limit ?? 20;
            var orders = query.Skip((arguments.PageNr ?? 0) * pageSize).Take(pageSize).ToList();
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