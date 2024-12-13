using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class PaymentService
{
    private readonly PaymentRepository _paymentRepository;
    private readonly OrderService _orderService;

    public PaymentService(PaymentRepository paymentRepository, OrderService orderService)
    {
        _paymentRepository = paymentRepository;
        _orderService = orderService;
    }

    public List<PaymentModel> GetPayments(PaymentGetDto paymentGetDto)
    {
        paymentGetDto.page_nr = 0;
        paymentGetDto.limit = 20;
        var query = _paymentRepository.GetPayments(paymentGetDto);
        var gottenPayments = query.Skip(paymentGetDto.page_nr * paymentGetDto.limit).Take(paymentGetDto.limit).ToList();
        return gottenPayments;
    }

    public PaymentModel GetPaymentById(int paymentId)
    {
        PaymentModel result;
        try {
            result = _paymentRepository.GetPaymentById(paymentId);
        } catch (KeyNotFoundException ex) {
            throw;
        }
        
        return result;
    }

    public PaymentModel CreatePayment(PaymentCreateDto newPaymentDto)
    {
        if(newPaymentDto.payment_method == paymentMethodEnum.GIFTCARD && newPaymentDto.gift_card_id == null)
        {
            Console.WriteLine("Payment method was Gift Card, but gift card was not provided");
            throw new ValidationException("Payment method was Gift Card, but gift card was not provided");
        }

        decimal orderTotalFromOrderItems = this.getOrderTotalFromOrderItems(newPaymentDto.order_id);
        decimal alreadyPaid = 0;
        
        List<PaymentModel> paymentsForOrder = _paymentRepository.getPaymentsForOrder(newPaymentDto.order_id);

        foreach(PaymentModel payment in paymentsForOrder)
        {
            alreadyPaid += payment.total_amount ?? 0;
        }

        OrderModel? currentOrder = _orderService.GetOrder(newPaymentDto.order_id);


        decimal orderTotal = currentOrder.total_amount ?? 0m;

        // there are 2 comparisons, because it is not clear if order.total_amount is the same as the sum or prices from orderItems
        // inside that order, since menu managment is not implemented yet.
        if(orderTotal - alreadyPaid < newPaymentDto.total_amount && orderTotalFromOrderItems - alreadyPaid < newPaymentDto.total_amount)
        {
            Console.WriteLine($"Payment total amount exceeds the amount left to be paid for this order");
            throw new ValidationException("Payment total amount exceeds the amount left to be paid for this order");
        }

        if(currentOrder.order_status == "CLOSED")
        {
            Console.WriteLine("Cannot add a payment for a closed order");
            throw new ValidationException("Cannot add a payment for a closed order");
        }
        
        PaymentModel newPaymentModel = new PaymentModel();
        newPaymentModel.id = _paymentRepository.GetNewPaymentId(); 
        newPaymentModel.business_id = null; // TO-DO add business authorization
        newPaymentModel.order_id = newPaymentDto.order_id;
        newPaymentModel.total_amount = newPaymentDto.total_amount;
        newPaymentModel.order_amount = null; // TO-DO what is order_amount
        newPaymentModel.tip_amount = newPaymentDto.tip_amount;
        newPaymentModel.payment_method = newPaymentDto.payment_method;
        newPaymentModel.payment_status = paymentStatusEnum.DONE;
        newPaymentModel.created_at = DateTime.Now;
        newPaymentModel.gift_card_id = null; // TO-DO implement gift cards
        
        PaymentModel result;

        try {
            result = _paymentRepository.CreatePayment(newPaymentModel);

            // there are 2 comparisons, because it is not clear if order.total_amount is the same as the sum or prices from orderItems
            // inside that order, since menu managment is not implemented yet.
            if(orderTotal == alreadyPaid+newPaymentDto.total_amount || orderTotalFromOrderItems == alreadyPaid+newPaymentDto.total_amount)
            {
                Console.WriteLine("Closing order");
                _orderService.closeOrder(newPaymentDto.order_id);
            }
        } catch (DbUpdateException ex) {
            throw;
        }

        return result;
    }

    public PaymentModel UpdatePayment(int paymentId, PaymentUpdateDto updatedPaymentDto)
    {
        if(updatedPaymentDto.payment_method == paymentMethodEnum.GIFTCARD && updatedPaymentDto.gift_card_id == null)
        {
            Console.WriteLine("Payment method was Gift Card, but gift card was not provided");
            throw new ValidationException("Payment method was Gift Card, but gift card was not provided");
        }

        decimal orderTotalFromOrderItems = this.getOrderTotalFromOrderItems(updatedPaymentDto.order_id);
        decimal alreadyPaid = 0;
        
        List<PaymentModel> paymentsForOrder = _paymentRepository.getPaymentsForOrder(updatedPaymentDto.order_id);

        foreach(PaymentModel payment in paymentsForOrder)
        {
            alreadyPaid += payment.total_amount ?? 0;
        }

        PaymentModel currentPayment;

        try{
            currentPayment = GetPaymentById(paymentId);
        } catch (KeyNotFoundException ex) {
            throw;
        }

        decimal alreadyPaidWithoutUpdatedPayment = alreadyPaid - currentPayment.total_amount ?? 0;

        
        OrderModel? currentOrder; 
        try{
            currentOrder = _orderService.GetOrder(updatedPaymentDto.order_id);
        } catch (KeyNotFoundException ex){
            throw;
        }
        

        if(alreadyPaidWithoutUpdatedPayment + updatedPaymentDto.total_amount > currentOrder.total_amount && alreadyPaidWithoutUpdatedPayment + updatedPaymentDto.total_amount > orderTotalFromOrderItems)
        {
            Console.WriteLine("Updated payment makes the total payment of order exceed the total order amount");
            throw new ValidationException("Updated payment makes the total payment of order exceed the total order amount");
        }

        if(currentOrder.order_status == "CLOSED")
        {
            Console.WriteLine("Cannot edit a payment for a closed order");
            throw new ValidationException("Cannot edit a payment for a closed order");
        }

        PaymentModel result;
        try {
            result = _paymentRepository.UpdatePayment(paymentId, updatedPaymentDto);

            if(currentOrder.total_amount == alreadyPaidWithoutUpdatedPayment + updatedPaymentDto.total_amount || alreadyPaidWithoutUpdatedPayment + updatedPaymentDto.total_amount == orderTotalFromOrderItems)
            {
                _orderService.closeOrder(updatedPaymentDto.order_id);
            }
        } catch (DbUpdateException) {
            throw;
        }

        return result;
    }

    // iterates over order items of order to get the total price
    public decimal getOrderTotalFromOrderItems(int orderId)
    {
        decimal totalAmount = 0;
        // TO-DO limit order to not have more that 1000 orderitems
        IEnumerable<OrderItemModel> itemsInOrder = _orderService.GetItems(orderId, 0, 1000);

        foreach(OrderItemModel item in itemsInOrder)
        {
            if(item.product_price == null)
            {
                Console.WriteLine($"Warning: Product price is null for item with ID {item.id} in order {orderId}");
            }
            totalAmount += item.product_price ?? 0m;
        }

        return totalAmount;
    }
}