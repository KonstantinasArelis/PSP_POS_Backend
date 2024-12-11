using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class PaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<PaymentModel> GetPayments(PaymentGetDto paymentGetDto)
    {
        var query = _context.Payment.AsQueryable();

        if (paymentGetDto.order_id != null)
            query = query.Where(p => p.order_id == paymentGetDto.order_id);
            /*
        if (paymentGetDto.payment_method != null)
            query = query.Where(p => p.payment_method == paymentGetDto.payment_method);
        if (paymentGetDto.created_before != null)
            query = query.Where(p => p.created_at <= paymentGetDto.created_before);
        if (paymentGetDto.created_after != null)
            query = query.Where(p => p.created_at >= paymentGetDto.created_after);
        if (paymentGetDto.status != null)
            query = query.Where(p => p.payment_status == paymentGetDto.status); 
            */
        return query;
    }

    public PaymentModel GetPaymentById(int paymentId)
    {

        PaymentModel? result = _context.Payment.Single(p => p.id == paymentId);

        if (result == null) {
            throw new KeyNotFoundException("Payment with id " + paymentId + " was not found");
        }
        
        return result;
    }

    public PaymentModel CreatePayment(PaymentModel newPayment)
    {
        _context.Payment.Add(newPayment);
        //The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value. The statement has been terminated.
        int rowsAffected = _context.SaveChanges();
        if(rowsAffected == 0){
            throw new DbUdateException("Failed to create payment in database");
        }
        return newPayment;
    }

    public PaymentModel UpdatePayment(int paymentId, PaymentUpdateDto updatedPaymentDto)
    {
        PaymentModel payment = GetPaymentById(paymentId);

        if (updatedPaymentDto.order_id != null)
            payment.order_id = updatedPaymentDto.order_id.Value;
        if (updatedPaymentDto.total_amount != null)
            payment.total_amount = updatedPaymentDto.total_amount.Value;
        if (updatedPaymentDto.order_amount != null)
            payment.order_amount = updatedPaymentDto.order_amount.Value;
        if (updatedPaymentDto.tip_amount != null)
            payment.tip_amount = updatedPaymentDto.tip_amount.Value;
        if (updatedPaymentDto.payment_method != null)
            payment.payment_method = updatedPaymentDto.payment_method.Value;
        if (updatedPaymentDto.created_at != null)
            payment.created_at = updatedPaymentDto.created_at.Value;
        if (updatedPaymentDto.status != null)
            payment.payment_status = updatedPaymentDto.status.Value;
        if (updatedPaymentDto.gift_card_id != null)
            payment.gift_card_id = updatedPaymentDto.gift_card_id.Value;

        result = _context.SaveChanges();

        if(result == 0)
        {
            throw new DbUpdateException("Failed to update payment in database");
        }

        return payment;
    }

    public int GetNewPaymentId()
    {
        return _context.Payment.Select(p => p.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}