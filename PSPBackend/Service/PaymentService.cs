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
        

        if (newPaymentDto.order_id != null)
            newPaymentModel.order_id = newPaymentDto.order_id.Value;
        if (newPaymentDto.total_amount != null)
            newPaymentModel.total_amount = newPaymentDto.total_amount.Value;
        if (newPaymentDto.order_amount != null)
            newPaymentModel.order_amount = newPaymentDto.order_amount.Value;
        if (newPaymentDto.tip_amount != null)
            newPaymentModel.tip_amount = newPaymentDto.tip_amount.Value;
        if (newPaymentDto.payment_method != null)
            newPaymentModel.payment_method = newPaymentDto.payment_method.Value;
        if (newPaymentDto.created_at != null)
            newPaymentModel.created_at = newPaymentDto.created_at.Value;
        if (newPaymentDto.status != null)
            newPaymentModel.payment_status = newPaymentDto.status.Value;
        if (newPaymentDto.gift_card_id != null)
            newPaymentModel.gift_card_id = newPaymentDto.gift_card_id.Value; 

        var result = _paymentRepository.CreatePayment(newPaymentModel);
        return 0;
    }

    public int UpdatePayment(int paymentId, PaymentUpdateDto updatedPaymentDto)
    {
        var result = _paymentRepository.UpdatePayment(paymentId, updatedPaymentDto);
        return result;
    }
}