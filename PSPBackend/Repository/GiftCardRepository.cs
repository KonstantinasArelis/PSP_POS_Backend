using Microsoft.EntityFrameworkCore;
using PSPBackend.Model;

public class GiftCardRepository
{
    private readonly AppDbContext _context;

    public GiftCardRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<GiftCardModel> GetGiftCards(GiftCardGetDto giftCardGetDto)
    {
        var query = _context.GiftCard.AsQueryable(); // Assuming "GiftCard" is your DbSet

        if (giftCardGetDto.original_amount_atleast > 0)
            query = query.Where(g => g.original_amount >= giftCardGetDto.original_amount_atleast);
        if (giftCardGetDto.original_amount_lessthan > 0)
            query = query.Where(g => g.original_amount < giftCardGetDto.original_amount_lessthan);
        if (giftCardGetDto.amount_left_atleast > 0)
            query = query.Where(g => g.amount_left >= giftCardGetDto.amount_left_atleast);
        if (giftCardGetDto.amount_left_lessthan > 0)
            query = query.Where(g => g.amount_left < giftCardGetDto.amount_left_lessthan);
        if (giftCardGetDto.valid_starting_from != DateTime.MinValue)
            query = query.Where(g => g.valid_from >= giftCardGetDto.valid_starting_from);
        if (giftCardGetDto.valid_atleast_until != DateTime.MinValue)
            query = query.Where(g => g.valid_until >= giftCardGetDto.valid_atleast_until);
        if (!string.IsNullOrEmpty(giftCardGetDto.code_hash))
            query = query.Where(g => g.code_hash == giftCardGetDto.code_hash);

        return query;
    }

    public GiftCardModel GetGiftCardById(int giftCardId)
    {
        GiftCardModel? gottenGiftCard = _context.GiftCard.Single(g => g.id == giftCardId);
        return gottenGiftCard;
    }

    public int CreateGiftCard(GiftCardModel newGiftCard)
    {
        _context.GiftCard.Add(newGiftCard);
        int rowsAffected = _context.SaveChanges();
        return rowsAffected;
    }

    public int UpdateGiftCard(int giftCardId, GiftCardUpdateDto updatedGiftCardDto)
    {
        GiftCardModel giftCard = GetGiftCardById(giftCardId);

        if (updatedGiftCardDto.amount_left != null)
            giftCard.amount_left = updatedGiftCardDto.amount_left.Value;
        if (updatedGiftCardDto.valid_until != null)
            giftCard.valid_until = updatedGiftCardDto.valid_until.Value;

        return _context.SaveChanges();
    }

    public int GetNewGiftCardId()
    {
        return _context.GiftCard.Select(g => g.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}