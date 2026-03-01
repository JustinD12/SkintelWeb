using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkintelWeb.Data;
using SkintelWeb.Models;

namespace SkintelWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly SkintelDbContext _db;
    public BrandsController(SkintelDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var query = _db.Brands.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(b => b.Name.Contains(search) || b.Country.Contains(search) || b.ParentCompany.Contains(search));
        return Ok(await query.OrderBy(b => b.Name).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _db.Brands.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Brand brand)
    {
        brand.CreatedAt = DateTime.Now.ToString("o");
        _db.Brands.Add(brand);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Brand updated)
    {
        var item = await _db.Brands.FindAsync(id);
        if (item == null) return NotFound();
        item.Name = updated.Name; item.Country = updated.Country;
        item.ParentCompany = updated.ParentCompany; item.PhDistributor = updated.PhDistributor;
        item.OfficialWebsite = updated.OfficialWebsite; item.BatchCodeFormat = updated.BatchCodeFormat;
        item.BatchCodeRegex = updated.BatchCodeRegex; item.BarcodePrefixes = updated.BarcodePrefixes;
        item.AuthorizedSellers = updated.AuthorizedSellers; item.AuthenticIndicators = updated.AuthenticIndicators;
        item.CounterfeitWarnings = updated.CounterfeitWarnings;
        item.PriceMin = updated.PriceMin; item.PriceMax = updated.PriceMax;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Brands.FindAsync(id);
        if (item == null) return NotFound();
        _db.Brands.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("validate")]
    public async Task<IActionResult> Validate([FromQuery] string brandName, [FromQuery] string? batchCode, [FromQuery] string? price)
    {
        var brand = await _db.Brands.FirstOrDefaultAsync(b => b.Name.ToLower().Contains(brandName.ToLower()));
        if (brand == null) return Ok(new { found = false, message = "Brand not in registry" });

        var results = new List<object>();

        // Batch code validation
        if (!string.IsNullOrEmpty(batchCode))
        {
            try {
                var valid = System.Text.RegularExpressions.Regex.IsMatch(batchCode.ToUpper(), brand.BatchCodeRegex);
                results.Add(new { check = "Batch Code", valid, message = valid ? $"✅ Matches {brand.Name} format" : $"⚠️ Expected: {brand.BatchCodeFormat}" });
            } catch { results.Add(new { check = "Batch Code", valid = false, message = "Could not validate" }); }
        }

        // Price validation
        if (!string.IsNullOrEmpty(price) && double.TryParse(price.Replace("₱","").Replace(",",""), out var numPrice))
        {
            var withinRange = numPrice >= brand.PriceMin && numPrice <= brand.PriceMax * 1.3;
            var highRisk = numPrice < brand.PriceMin * 0.5;
            results.Add(new { check = "Price Range", valid = withinRange, message = highRisk ? $"🚨 Price ₱{numPrice} far below range ₱{brand.PriceMin}–₱{brand.PriceMax}" : withinRange ? $"✅ Price within normal range ₱{brand.PriceMin}–₱{brand.PriceMax}" : $"⚠️ Price below expected range ₱{brand.PriceMin}–₱{brand.PriceMax}" });
        }

        return Ok(new {
            found = true,
            brand = new { brand.Name, brand.Country, brand.ParentCompany, brand.PhDistributor, brand.OfficialWebsite },
            authorizedSellers = brand.AuthorizedSellers.Split(','),
            authenticIndicators = brand.AuthenticIndicators.Split(','),
            counterfeitWarnings = brand.CounterfeitWarnings.Split(','),
            priceRange = new { min = brand.PriceMin, max = brand.PriceMax },
            validationResults = results
        });
    }
}
