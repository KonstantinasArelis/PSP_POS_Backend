using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using PSPBackend.Model;
using PSPBackend.Dto;
public class OrderService
{
        private readonly OrderRepository _orderRepository;
        private readonly TaxService _taxService;
        public OrderService(OrderRepository orderRepository, TaxService taxService)
        {
            _orderRepository = orderRepository;
            _taxService = taxService;
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

        public List<OrderModel> GetOrders(OrderGetDto arguments)
        {
            Console.WriteLine("LOG: Order service GetOrders");
            var query = _orderRepository.GetOrders(arguments);
            var pageSize = arguments.limit ?? 50;
            var orders = query.Skip((arguments.page_nr ?? 0) * pageSize).Take(pageSize).ToList();
            return orders;
        }

        public OrderModel? GetOrder(int orderId)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            return _orderRepository.GetOrder(orderId);
        }

        public OrderModel? UpdateOrderStatus(int orderId, string status)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order == null) return null;
            order.order_status = status;
            if(order.order_status == "CLOSED" && order.closed_at == null) order.closed_at = DateTime.Now; 
            var returnOrder = _orderRepository.UpdateOrder(order);
            return returnOrder;
        }

        public void RecalculateOrder(int orderId)
        {
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order == null) return;
            decimal newTotalAmount = 0;
            decimal newTaxAmount = 0;
            decimal newOrderDiscountPercentage = 0;
            decimal newTotalDiscountAmount = 0;
            foreach(OrderItemModel item in order.items){
                decimal itemTax = 0;
                if(item.tax_id != null){
                    var tax = _taxService.GetTax((int) item.tax_id);
                    if(tax != null && tax.tax_rate != null){
                        itemTax = (decimal) tax.tax_rate;
                    }
                }
                if(item.quantity != null && item.quantity != 0){
                    decimal quantity = (decimal) item.quantity;
                    newTotalAmount += quantity * (item.product_price ?? 0 + item.variation_price ?? 0 + itemTax - item.item_discount_amount ?? 0);
                    newTaxAmount += quantity * itemTax;
                    newTotalDiscountAmount += quantity * item.item_discount_amount ?? 0;
                }
            }
            //newOrderDiscountPercentage calculations go here probably
            order.total_amount = newTotalAmount;
            order.tax_amount = newTaxAmount;
            order.total_discount_amount = newTotalDiscountAmount;
            //order.order_discount_percentage = newOrderDiscountPercentage;
            _orderRepository.UpdateOrder(order);
        }

        public void UpdateOrderDiscount(int orderId, string bodyString)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order == null) return;
            dynamic? obj = JsonConvert.DeserializeObject<dynamic>(bodyString);
            if(obj != null)
            {
                try
                {
                    order.order_discount_percentage = obj.order_discount_percentage;
                    _orderRepository.UpdateOrder(order); 
                }
                catch(RuntimeBinderException){}
            }
        }

        public void DeleteOrder(int orderId)
        {
            _orderRepository.DeleteOrder(orderId);
        }

        public IEnumerable<OrderItemModel> GetItems(int orderId, int? pageNr, int? limit)
        {
            var query = _orderRepository.GetOrderItems(orderId);
            var pageSize = limit ?? 50;
            return query.Skip((pageNr ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public OrderItemModel? GetItem(int orderId, int itemId)
        {
            Console.WriteLine("LOG: Order service GetOrder");
            var item =  _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId) return item;
            return null;
        }

        public OrderItemModel? AddItem(int orderId, OrderItemModel item)
        {
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order != null)
            {
                if(item.id == 0) item.id = _orderRepository.GetNewOrderItemId();
                item.order_id = orderId;
                var addedItem = _orderRepository.AddOrderItem(item);
                if(addedItem != null) RecalculateOrder(orderId);
                return addedItem;
            }
            return null;
        }

        public OrderItemModel? AddItem(int orderId, OrderItemCreateDto itemDto)
        {
            OrderModel? order = _orderRepository.GetOrder(orderId);
            if(order != null)
            {
                int newId = _orderRepository.GetNewOrderItemId();
                string? newProductName = null; //TODO: figure out the product name when menu stuff is done
                decimal? newProductPrice = null; //TODO: figure out the product price when menu stuff is done
                int? newTaxId = null; //TODO: figure out the item's taxes when tax stuff is done
                decimal newVariationPrice = 0;
                foreach(var variation in itemDto.variations){
                    newVariationPrice += variation.price;
                }
                decimal? newItemDiscountAmount = null; //TODO: figure out the item's discount when discount stuff is done
                OrderItemModel newItem = new OrderItemModel(){
                    id=newId, 
                    order_id=orderId,
                    product_id=itemDto.product_id,
                    reservation_id = null,
                    quantity = itemDto.quantity,
                    variations = JsonConvert.SerializeObject(itemDto.variations),
                    product_name = newProductName,
                    product_price = newProductPrice,
                    tax_id = newTaxId,
                    variation_price = newVariationPrice,
                    item_discount_amount = newItemDiscountAmount
                };
                var addedItem = _orderRepository.AddOrderItem(newItem);
                if(addedItem != null) RecalculateOrder(orderId);
                return addedItem;
            }
            return null;
        }

        public OrderItemModel? UpdateItem(int orderId, int itemId, OrderItemUpdateDto updateDto)
        {
            OrderItemModel? item = _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId)
            {
                string variationsReserialized = JsonConvert.SerializeObject(updateDto.variations);
                decimal variationPrice = 0;
                if(updateDto.variations != null)
                {
                    foreach(VariationDto variation in updateDto.variations){
                        variationPrice += variation.price;
                    }
                }
                item.quantity = updateDto.quantity;
                item.variations = variationsReserialized;
                item.variation_price = variationPrice;
                item = _orderRepository.UpdateOrderItem(item);
                RecalculateOrder(orderId);
                return item;
            }
            return null;
        }

        public void DeleteItem(int orderId, int itemId)
        {
            OrderItemModel? item = _orderRepository.GetOrderItem(itemId);
            if(item != null && item.order_id == orderId)
            {
                _orderRepository.DeleteOrderItem(itemId);
                RecalculateOrder(orderId);
            }
        }

        public OrderItemModel getOrderItemByReservationId(int reservationId)
        {
            OrderItemModel result = _orderRepository.getOrderItemByReservationId(reservationId);
            return result;
        }

        public int closeOrder(int orderId)
        {
            int result =_orderRepository.closeOrder(orderId);
            return result;
        }
}