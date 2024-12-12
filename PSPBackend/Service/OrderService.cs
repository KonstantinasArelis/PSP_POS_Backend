using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using PSPBackend.Model;
using PSPBackend.Utility;
public class OrderService
{
        private readonly OrderRepository _orderRepository;
        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;  
        }

        public OrderModel? CreateOrder(OrderModel order)
        {
            Console.WriteLine("LOG: service creates order id is " + order.id);
            if(order.id == 0)
            {
                order.id = _orderRepository.GetNewOrderId();
            }
            if(order.created_at == null) order.created_at = DateTime.Now;
            return _orderRepository.AddOrder(order);
        }

        public List<OrderModel> GetOrders(OrderArgumentModel arguments)
        {
            Console.WriteLine("LOG: Order service GetOrders");
            var query = _orderRepository.GetOrders(arguments);
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
                    order.order_status = obj.status;
                    if(order.order_status == "CLOSED" && order.closed_at == null) order.closed_at = DateTime.Now; 
                    _orderRepository.UpdateOrder(order); 
                }
                catch(RuntimeBinderException){}
            }
        }

        public void DeleteOrder(int orderId)
        {
            _orderRepository.DeleteOrder(orderId);
        }

        public OrderItemModel? AddItem(int orderId, OrderItemModel item)
        {
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order != null)
            {
                if(item.id == 0) item.id = _orderRepository.GetNewOrderItemId();
                item.order_id = orderId;
                return _orderRepository.AddOrderItem(item);
            }
            return null;
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
                        variationPrice += variation.price;
                    }
                    item.quantity = update.quantity;
                    item.variations = variationsReserialized;
                    item.variation_price = variationPrice;
                    _orderRepository.UpdateOrderItem(item);
                }
            }
        }

        public void DeleteItem(int orderId, int itemId)
        {
            OrderItemModel? item = _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId)
            {
                _orderRepository.DeleteOrderItem(itemId);
            }
        }

        public OrderItemModel getOrderItemByReservationId(int reservationId)
        {
            OrderItemModel result = _orderRepository.getOrderItemByReservationId(reservationId);
            return result;
        }
}