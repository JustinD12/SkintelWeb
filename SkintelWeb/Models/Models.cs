using System.ComponentModel.DataAnnotations;

namespace SkintelWeb.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = "";
        [StringLength(150)]
        public string InciName { get; set; } = "";
        [StringLength(200)]
        public string Function { get; set; } = "";
        [Required]
        public string Rating { get; set; } = "safe"; // safe | caution | avoid
        [StringLength(500)]
        public string Notes { get; set; } = "";
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }

    public class Brand
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = "";
        [StringLength(100)]
        public string Country { get; set; } = "";
        [StringLength(150)]
        public string ParentCompany { get; set; } = "";
        [StringLength(200)]
        public string OfficialWebsite { get; set; } = "";
        [StringLength(200)]
        public string PhDistributor { get; set; } = "";
        [StringLength(200)]
        public string BatchCodeFormat { get; set; } = "";
        [StringLength(100)]
        public string BatchCodeRegex { get; set; } = "";
        [StringLength(500)]
        public string BarcodePrefixes { get; set; } = "";
        [StringLength(1000)]
        public string AuthenticIndicators { get; set; } = "";
        [StringLength(1000)]
        public string CounterfeitWarnings { get; set; } = "";
        public int PriceMin { get; set; }
        public int PriceMax { get; set; }
        [StringLength(500)]
        public string AuthorizedSellers { get; set; } = "";
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }

    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = "";
        [Required, StringLength(100)]
        public string Brand { get; set; } = "";
        [StringLength(50)]
        public string Barcode { get; set; } = "";
        [StringLength(100)]
        public string Category { get; set; } = "";
        [StringLength(2000)]
        public string Ingredients { get; set; } = "";
        public string Status { get; set; } = "authentic"; // authentic | flagged | counterfeit
        public string SafetyRating { get; set; } = "safe"; // safe | caution | avoid
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        [StringLength(500)]
        public string Notes { get; set; } = "";
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }
}
