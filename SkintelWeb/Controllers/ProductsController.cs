using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkintelWeb.Data;
using SkintelWeb.Models;

namespace SkintelWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly SkintelDbContext _db;
    public ProductsController(SkintelDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? status)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search) || p.Barcode.Contains(search));
        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);
        return Ok(await query.OrderBy(p => p.Name).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.Products.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> GetByBarcode(string barcode)
    {
        var item = await _db.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);
        return item == null ? NotFound(new { message = "Product not found in registry" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        product.CreatedAt = DateTime.Now.ToString("o");
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product updated)
    {
        var item = await _db.Products.FindAsync(id);
        if (item == null) return NotFound();
        item.Name = updated.Name; item.Brand = updated.Brand;
        item.Category = updated.Category; item.Status = updated.Status;
        item.Ingredients = updated.Ingredients; item.SafetyRating = updated.SafetyRating;
        item.Barcode = updated.Barcode; item.ImageUrl = updated.ImageUrl;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Products.FindAsync(id);
        if (item == null) return NotFound();
        _db.Products.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> Stats()
    {
        return Ok(new {
            total = await _db.Products.CountAsync(),
            authentic = await _db.Products.CountAsync(p => p.Status == "authentic"),
            flagged = await _db.Products.CountAsync(p => p.Status == "flagged"),
            counterfeit = await _db.Products.CountAsync(p => p.Status == "counterfeit")
        });
    }
}
