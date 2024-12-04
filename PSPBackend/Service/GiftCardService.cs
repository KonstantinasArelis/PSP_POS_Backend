using PSPBackend.Model;

public class GiftCardService
{
    private readonly GiftCardRepository _giftCardRepository;

    public GiftCardService(GiftCardRepository giftCardRepository)
    {
        _giftCardRepository = giftCardRepository;
    }

    public List<GiftCardModel> GetGiftCards(GiftCardGetDto giftCardGetDto)
    {
        var query = _giftCardRepository.GetGiftCards(giftCardGetDto);
        var gottenGiftCards = query
            .Skip(giftCardGetDto.page_nr * giftCardGetDto.limit)
            .Take(giftCardGetDto.limit)
            .ToList();
        return gottenGiftCards;
    }

    public GiftCardModel GetGiftCardById(int giftCardId)
    {
        var result = _giftCardRepository.GetGiftCardById(giftCardId);
        return result;
    }

    public int CreateGiftCard(GiftCardCreateDto newGiftCardDto)
    {
        GiftCardModel newGiftCardModel = new GiftCardModel();
        newGiftCardModel.id = _giftCardRepository.GetNewGiftCardId();

        if (newGiftCardDto.original_amount != null)
            newGiftCardModel.original_amount = newGiftCardDto.original_amount.Value;
        if (newGiftCardDto.valid_from != null)
            newGiftCardModel.valid_from = newGiftCardDto.valid_from.Value;
        if (newGiftCardDto.valid_until != null)
            newGiftCardModel.valid_until = newGiftCardDto.valid_until.Value;

        // Set amount_left to original_amount initially
        newGiftCardModel.amount_left = newGiftCardModel.original_amount; 

        // Generate code_hash (replace with your actual hash generation logic)
        newGiftCardModel.code_hash = GenerateCodeHash(); 

        var result = _giftCardRepository.CreateGiftCard(newGiftCardModel);
        return 0; // Consider returning a more meaningful value
    }

    public int UpdateGiftCard(int giftCardId, GiftCardUpdateDto updatedGiftCardDto)
    {
        var result = _giftCardRepository.UpdateGiftCard(giftCardId, updatedGiftCardDto);
        return result;
    }

    private string GenerateCodeHash()
    {
        return null;
    }
}