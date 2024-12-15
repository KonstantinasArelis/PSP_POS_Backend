using PSPBackend.Model;
public class DiscountService
{
        private readonly DiscountRepository _discountRepository;

        public DiscountService(DiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;  

        }

        public List<DiscountModel> GetDiscounts(
            int page_nr, int limit, string? name, string? type, DateTime? valid_starting_from,
            DateTime? valid_atleast_until, string? code_hash
        )
        {
            
            Console.WriteLine("LOG: Discount service GetDiscounts");
            var query = _discountRepository.GetDiscounts(
                name, type, valid_starting_from, valid_atleast_until, code_hash
            ); 

            var discounts = query.Skip(page_nr * limit).Take(limit).ToList();
            return discounts;
        }

        public DiscountModel? GetDiscount(int discountId)
        {
            
            Console.WriteLine("LOG: Discount service GetDiscount");
            var discount = _discountRepository.GetDiscount(discountId); 
            if(discount is null){
                return null;
            } else {
                return discount;
            }
        }

        public int? DeleteDiscount(int discountId)
        {
            Console.WriteLine("DeleteDiscount service");
            
            if (_discountRepository.DeleteDiscount(discountId) > 0){
                return 1; 
            } else {
                return null;
            }
        }

        public DiscountModel? UpdateDiscount(int discountId, DiscountModel discount)
        {
            Console.WriteLine("UpdateDiscount service");
            
            if (_discountRepository.UpdateDiscount(discountId, discount) > 0){
                return discount;
            } else {
                return null;
            }
        }

        public DiscountModel? CreateDiscount(DiscountModel discount)
        {
            Console.WriteLine("CreateDiscount service");
            if(discount.id == 0)
            {
                discount.id = _discountRepository.GetNewDiscountId();
            }

            if (_discountRepository.CreateDiscount(discount) > 0){
                return discount;
            } else {
                return null;
            }
        }
}