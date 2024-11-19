using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using PSPBackend.Model;
using PSPBackend.Utility;
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

        public OrderModel? GetOrder(int orderId)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            return _orderRepository.GetOrder(orderId);
        }

        public void UpdateOrderStatus(int orderId, string bodyString)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order == null) return;
            dynamic? obj = JsonConvert.DeserializeObject<dynamic>(bodyString);
            if(obj != null)
            {
                try
                {
                    string status = obj.status;
                    if(status != null)
                    {
                        int? order_status_int = _orderStatusRepository.ConvertNameToCode(status);
                        order.order_status = order_status_int;
                    }
                    _orderRepository.UpdateOrder(order); 
                }
                catch(RuntimeBinderException){}
            }
        }

        public void DeleteOrder(int orderId)
        {
            _orderRepository.DeleteOrder(orderId);
        }

        public void AddItem(int orderId, OrderItemModel item)
        {
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order != null)
            {
                item.order_id = orderId;
                _orderRepository.AddOrderItem(item);
            }
        }

        public IEnumerable<OrderItemModel> GetItems(int orderId, int? pageNr, int? limit)
        {
            var query = _orderRepository.GetOrderItems(orderId);
            var pageSize = limit ?? 20;
            return query.Skip((pageNr ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public OrderItemModel? GetItem(int orderId, int itemId)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            var item =  _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId) return item;
            return null;
        }

        public void UpdateItem(int orderId, int itemId, string bodyString)
        {
            OrderItemModel? item = _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId)
            {
                OrderItemUpdate? update = JsonConvert.DeserializeObject<OrderItemUpdate>(bodyString);
                if(update != null)
                {
                    string variationsReserialized = JsonConvert.SerializeObject(update.variations);
                    decimal variationPrice = 0;
                    foreach(Variation variation in update.variations){
                        variationPrice += variation.Price;
                    }
                    item.quantity = update.quantity;
                    item.variations = variationsReserialized;
                    item.variation_price = variationPrice;
                    _orderRepository.UpdateOrderItem(item);
                }
            }
        }
}