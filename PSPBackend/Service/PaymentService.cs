using System.ComponentModel.DataAnnotations;
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

        decimal alreadyPaid = this.getPaymentTotal(newPaymentDto.order_id);
        OrderModel? currentOrder = _orderService.GetOrder(newPaymentDto.order_id);


        decimal orderTotal = currentOrder.total_amount ?? 0m;
        if(orderTotal - alreadyPaid < newPaymentDto.total_amount)
        {
            Console.WriteLine($"Payment total amount exceeds the amount left to be paid for this order");
            throw new ValidationException("Payment total amount exceeds the amount left to be paid for this order");
        }
        
        PaymentModel newPaymentModel = new PaymentModel();
        newPaymentModel.id = _paymentRepository.GetNewPaymentId(); 
        newPaymentModel.business_id = null; // TO-DO add business authorization
        newPaymentModel.order_id = newPaymentDto.order_id;
        newPaymentModel.total_amount = newPaymentDto.total_amount;
        newPaymentModel.order_amount = null; // TO-DO what is order_amount
        newPaymentModel.tip_amount = newPaymentDto.tip_amount;
        newPaymentModel.payment_method = newPaymentDto.payment_method;

        newPaymentModel.created_at = DateTime.Now;
        newPaymentModel.payment_status = 0; // TO-DO implement payment flow
        newPaymentModel.gift_card_id = null; // TO-DO implement gift cards
        
        PaymentModel result;

        try {
            result = _paymentRepository.CreatePayment(newPaymentModel);
        } catch (dbUpdateException ex) {
            throw;
        }

        return result;
    }

    public PaymentModel UpdatePayment(int paymentId, PaymentUpdateDto updatedPaymentDto)
    {
        PaymentModel result;
        try {
            result = _paymentRepository.UpdatePayment(paymentId, updatedPaymentDto);
        } catch (DbUpdateException) {
            throw;
        }

        return result;
    }

    public decimal getPaymentTotal(int orderId)
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