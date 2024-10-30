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
}