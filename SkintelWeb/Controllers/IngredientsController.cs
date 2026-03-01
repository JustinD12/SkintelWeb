using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkintelWeb.Data;
using SkintelWeb.Models;

namespace SkintelWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly SkintelDbContext _db;
    public IngredientsController(SkintelDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? rating)
    {
        var query = _db.Ingredients.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(i => i.Name.Contains(search) || i.InciName.Contains(search) || i.Function.Contains(search));
        if (!string.IsNullOrEmpty(rating))
            query = query.Where(i => i.Rating == rating);
        return Ok(await query.OrderBy(i => i.Name).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.Ingredients.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Ingredient ingredient)
    {
        ingredient.CreatedAt = DateTime.Now.ToString("o");
        _db.Ingredients.Add(ingredient);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ingredient updated)
    {
        var item = await _db.Ingredients.FindAsync(id);
        if (item == null) return NotFound();
        item.Name = updated.Name;
        item.InciName = updated.InciName;
        item.Function = updated.Function;
        item.Rating = updated.Rating;
        item.Notes = updated.Notes;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Ingredients.FindAsync(id);
        if (item == null) return NotFound();
        _db.Ingredients.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> Stats()
    {
        return Ok(new {
            total = await _db.Ingredients.CountAsync(),
            safe = await _db.Ingredients.CountAsync(i => i.Rating == "safe"),
            caution = await _db.Ingredients.CountAsync(i => i.Rating == "caution"),
            avoid = await _db.Ingredients.CountAsync(i => i.Rating == "avoid")
        });
    }
}
