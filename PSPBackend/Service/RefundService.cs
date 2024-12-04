using PSPBackend.Model;

public class RefundService
{
    private readonly RefundRepository _refundRepository;

    public RefundService(RefundRepository refundRepository)
    {
        _refundRepository = refundRepository;
    }

    public List<RefundModel> GetRefunds(RefundGetDto refundGetDto)
    {
        var query = _refundRepository.GetRefunds(refundGetDto);
        var gottenRefunds = query
            .Skip(refundGetDto.page_nr.GetValueOrDefault() * refundGetDto.limit.GetValueOrDefault())
            .Take(refundGetDto.limit.GetValueOrDefault())
            .ToList();
        return gottenRefunds;
    }

    public RefundModel GetRefundById(int refundId)
    {
        var result = _refundRepository.GetRefundById(refundId);
        return result;
    }

    public int CreateRefund(RefundCreateDto newRefundDto)
    {
        RefundModel newRefundModel = new RefundModel();
        newRefundModel.id = _refundRepository.GetNewRefundId();

        // newRefundModel.business_id = ... 

        if (newRefundDto.order_item_id != null)
            newRefundModel.order_item_id = newRefundDto.order_item_id.Value;
        if (newRefundDto.returned_to_inventory != null)
            newRefundModel.returned_to_inventory = newRefundDto.returned_to_inventory.Value;
        if (newRefundDto.refunded_quantity != null)
            newRefundModel.refunded_quantity = newRefundDto.refunded_quantity.Value;
        if (newRefundDto.amount != null)
            newRefundModel.amount = newRefundDto.amount.Value;
        if (newRefundDto.reason != null)
            newRefundModel.reason = newRefundDto.reason;

        newRefundModel.created_at = DateTime.Now;

        var result = _refundRepository.CreateRefund(newRefundModel);
        return 0;
    }
}