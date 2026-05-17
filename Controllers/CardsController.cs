using Microsoft.AspNetCore.Mvc;
using ScrumBoardApi.Models;
using ScrumBoardApi.Services;

namespace ScrumBoardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly CardService _service;

    public CardsController(CardService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Card>> GetAll() => Ok(_service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<Card> GetById(Guid id)
    {
        var card = _service.GetById(id);
        return card is null ? NotFound() : Ok(card);
    }

    [HttpPost]
    public ActionResult<Card> Create(Card card)
    {
        var created = _service.Add(card);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<Card> Update(Guid id, Card card)
    {
        var updated = _service.Update(id, card);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:guid}/move")]
    public ActionResult<Card> Move(Guid id, [FromBody] MoveCardRequest request)
    {
        var moved = _service.MoveCard(id, request.Status);
        return moved is null ? NotFound() : Ok(moved);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}

public record MoveCardRequest(string Status);
