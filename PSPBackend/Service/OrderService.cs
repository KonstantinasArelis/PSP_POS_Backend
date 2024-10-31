using PSPBackend.Model;
public class OrderService
{
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;  

        }

        public string CreateOrder()
        {
            Console.WriteLine("LOG: service creates order");
            OrderModel testOrder = new OrderModel();
            return _orderRepository.AddOrder(testOrder);
        }

        public List<OrderModel> GetOrders(
            int page_nr = 0, int limit = 20, int? employee_id = null, 
            decimal? min_total_amount = null, decimal? max_total_amount = null, 
            string order_status = null
        )
        {
            Console.WriteLine("LOG: Order service GetOrders");
            //List<OrderModel> orders = new List<OrderModel>();
            var query = _orderRepository.GetOrders(employee_id, min_total_amount, 
                                          max_total_amount, order_status); 
            var pageSize = 20;
            var orders = query.Skip(page_nr * pageSize).Take(pageSize).ToList();
            return orders;
        }
}