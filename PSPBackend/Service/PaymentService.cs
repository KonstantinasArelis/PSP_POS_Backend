using PSPBackend.Model;

public class PaymentService
{
    private readonly PaymentRepository _paymentRepository;

    public PaymentService(PaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
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
        var result = _paymentRepository.GetPaymentById(paymentId);
        return result;
    }

    public int CreatePayment(PaymentCreateDto newPaymentDto)
    {
        PaymentModel newPaymentModel = new PaymentModel();
        newPaymentModel.id = _paymentRepository.GetNewPaymentId(); 

        // newPaymentModel.business_id = ? 
        
        if (newPaymentDto.total_amount != null)
            newPaymentModel.total_amount = newPaymentDto.total_amount;
        if (newPaymentDto.order_id != null)
            newPaymentModel.order_id = newPaymentDto.order_id;
        if (newPaymentDto.order_amount != null)
            newPaymentModel.order_amount = newPaymentDto.order_amount;
        if (newPaymentDto.tip_amount != null)
            newPaymentModel.tip_amount = newPaymentDto.tip_amount;
        if (newPaymentDto.payment_method != null)
            newPaymentModel.payment_method = newPaymentDto.payment_method;
        if (newPaymentDto.gift_card_id != null)
            newPaymentModel.gift_card_id = newPaymentDto.gift_card_id; 

        newPaymentModel.created_at = DateTime.Now;
        newPaymentModel.business_id = null;

        var result = _paymentRepository.CreatePayment(newPaymentModel);
        return result;
    }

    public int UpdatePayment(int paymentId, PaymentUpdateDto updatedPaymentDto)
    {
        var result = _paymentRepository.UpdatePayment(paymentId, updatedPaymentDto);
        return result;
    }
}