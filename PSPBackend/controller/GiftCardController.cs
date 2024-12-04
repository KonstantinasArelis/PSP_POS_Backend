using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.Model;

[ApiController]
[Route("[controller]")]
public class GiftCardController : ControllerBase
{
    private readonly GiftCardService _giftCardService;

    public GiftCardController(GiftCardService giftCardService)
    {
        _giftCardService = giftCardService;
    }

    [HttpGet]
    public IActionResult GetGiftCards([FromQuery] GiftCardGetDto giftCardGetDto)
    {
        List<GiftCardModel> gottenGiftCards = _giftCardService.GetGiftCards(giftCardGetDto);
        return Ok(gottenGiftCards);
    }

    [HttpGet("{id}")]
    public IActionResult GetGiftCardById([FromRoute] int id)
    {
        Console.WriteLine("GiftCard controller GetGiftCardById with id: " + id);
        var gottenGiftCard = _giftCardService.GetGiftCardById(id);
        return Ok(gottenGiftCard);
    }

    [HttpPost]
    public IActionResult CreateGiftCard([FromBody] GiftCardCreateDto newGiftCard)
    {
        var createdGiftCard = _giftCardService.CreateGiftCard(newGiftCard);
        return Ok(createdGiftCard);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateGiftCard([FromRoute] int giftCardId, [FromBody] GiftCardUpdateDto updatedGiftCardDto)
    {
        var updatedGiftCard = _giftCardService.UpdateGiftCard(giftCardId, updatedGiftCardDto);
        return Ok(updatedGiftCard);
    }
}