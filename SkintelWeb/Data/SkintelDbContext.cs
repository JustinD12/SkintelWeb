using Microsoft.EntityFrameworkCore;
using SkintelWeb.Models;

namespace SkintelWeb.Data;

public class SkintelDbContext : DbContext
{
    public SkintelDbContext(DbContextOptions<SkintelDbContext> options) : base(options) { }

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Ingredients
        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Name = "Niacinamide", InciName = "Niacinamide", Function = "Brightening, pore-minimizing, oil control", Rating = "safe", Notes = "Effective at 2-10%. Well tolerated by most skin types." },
            new Ingredient { Id = 2, Name = "Retinol", InciName = "Retinol", Function = "Anti-aging, cell turnover, acne", Rating = "caution", Notes = "Start low (0.025%). Avoid during pregnancy. Use at night only." },
            new Ingredient { Id = 3, Name = "Sodium Lauryl Sulfate", InciName = "Sodium Lauryl Sulfate", Function = "Surfactant / cleansing agent", Rating = "avoid", Notes = "Known irritant. Can disrupt skin barrier. Especially harsh for sensitive skin." },
            new Ingredient { Id = 4, Name = "Hyaluronic Acid", InciName = "Sodium Hyaluronate", Function = "Hydration, plumping", Rating = "safe", Notes = "Attracts and binds moisture. Safe for all skin types including sensitive." },
            new Ingredient { Id = 5, Name = "Vitamin C", InciName = "Ascorbic Acid", Function = "Antioxidant, brightening", Rating = "caution", Notes = "Unstable at high pH. Can irritate sensitive skin. Use in AM routine." },
            new Ingredient { Id = 6, Name = "Ceramides", InciName = "Ceramide NP / EOP / AP", Function = "Barrier repair, moisturizing", Rating = "safe", Notes = "Essential for skin barrier. Excellent for dry and sensitive skin." },
            new Ingredient { Id = 7, Name = "Fragrance", InciName = "Parfum / Fragrance", Function = "Scent", Rating = "avoid", Notes = "Common allergen and sensitizer. Avoid for sensitive or reactive skin." },
            new Ingredient { Id = 8, Name = "Salicylic Acid", InciName = "Salicylic Acid", Function = "Exfoliant, acne treatment", Rating = "caution", Notes = "BHA exfoliant. Effective for oily and acne-prone skin. Avoid during pregnancy." },
            new Ingredient { Id = 9, Name = "Zinc Oxide", InciName = "Zinc Oxide", Function = "Sunscreen, anti-inflammatory", Rating = "safe", Notes = "Physical UV filter. Safe for sensitive skin. Effective broad-spectrum protection." },
            new Ingredient { Id = 10, Name = "Parabens", InciName = "Methylparaben / Propylparaben", Function = "Preservative", Rating = "caution", Notes = "Controversial. EU restricts some parabens. Generally safe at low concentrations." },
            new Ingredient { Id = 11, Name = "Glycerin", InciName = "Glycerin", Function = "Humectant, moisturizing", Rating = "safe", Notes = "Draws moisture to skin. Safe for all skin types. Very commonly used." },
            new Ingredient { Id = 12, Name = "Centella Asiatica", InciName = "Centella Asiatica Extract", Function = "Soothing, healing, anti-inflammatory", Rating = "safe", Notes = "Popular in K-beauty. Excellent for sensitive and acne-prone skin." },
            new Ingredient { Id = 13, Name = "Benzoyl Peroxide", InciName = "Benzoyl Peroxide", Function = "Acne treatment, antibacterial", Rating = "caution", Notes = "Effective for acne but can bleach fabrics and cause dryness. Start at 2.5%." },
            new Ingredient { Id = 14, Name = "Mineral Oil", InciName = "Paraffinum Liquidum", Function = "Emollient, occlusive", Rating = "caution", Notes = "Comedogenic for some. Highly refined cosmetic grade is considered safe." },
            new Ingredient { Id = 15, Name = "Snail Secretion Filtrate", InciName = "Snail Secretion Filtrate", Function = "Repair, hydration, brightening", Rating = "safe", Notes = "Popular K-beauty ingredient. Supports skin repair and hydration." }
        );

        // Seed Brands
        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Cetaphil", Country = "USA", ParentCompany = "Galderma", PhDistributor = "Galderma Philippines", OfficialWebsite = "cetaphil.com", BatchCodeFormat = "Alphanumeric 6-10 chars on bottom", BatchCodeRegex = "^[A-Z]{1,3}[0-9]{4,7}$", BarcodePrefixes = "305950,302994,302993", AuthorizedSellers = "Watson's,SM Beauty,Rose Pharmacy,Mercury Drug", AuthenticIndicators = "Galderma logo,Lot number on bottom,Made in Canada/USA", CounterfeitWarnings = "Missing Galderma logo,Price below ₱300 for 250ml,Blurry label", PriceMin = 350, PriceMax = 1200 },
            new Brand { Id = 2, Name = "CeraVe", Country = "USA", ParentCompany = "L'Oreal", PhDistributor = "L'Oreal Philippines", OfficialWebsite = "cerave.com", BatchCodeFormat = "Numbers and letters embossed on bottom", BatchCodeRegex = "^[0-9]{8}[A-Z]$", BarcodePrefixes = "301871,301872", AuthorizedSellers = "Watson's,SM Beauty,Robinsons,Lazada Mall", AuthenticIndicators = "MVE Technology on label,L'Oreal mark,Fragrance-free label", CounterfeitWarnings = "No MVE Technology,Spelling errors,Price below ₱400", PriceMin = 450, PriceMax = 1500 },
            new Brand { Id = 3, Name = "NIVEA", Country = "Germany", ParentCompany = "Beiersdorf", PhDistributor = "Beiersdorf Philippines", OfficialWebsite = "nivea.com", BatchCodeFormat = "4 digits + letter on tin bottom", BatchCodeRegex = "^[0-9]{4}[A-Z]$", BarcodePrefixes = "4005808,4006000", AuthorizedSellers = "7-Eleven,Alfamart,SM,Watson's,Mercury Drug", AuthenticIndicators = "NIVEA embossed on tin,Beiersdorf logo,Consistent blue color", CounterfeitWarnings = "Lid not embossed,Off-shade blue,Price below ₱150", PriceMin = 150, PriceMax = 600 },
            new Brand { Id = 4, Name = "Neutrogena", Country = "USA", ParentCompany = "Johnson & Johnson", PhDistributor = "J&J Philippines", OfficialWebsite = "neutrogena.com", BatchCodeFormat = "Alphanumeric on box and bottle", BatchCodeRegex = "^[A-Z][0-9]{6}[A-Z]$", BarcodePrefixes = "070501,086800", AuthorizedSellers = "Watson's,Mercury Drug,SM,Shopee Mall", AuthenticIndicators = "J&J logo,Dermatologist recommended seal,Clear expiry date", CounterfeitWarnings = "J&J logo missing,Inconsistent fonts,Price below ₱250", PriceMin = 250, PriceMax = 900 },
            new Brand { Id = 5, Name = "Garnier", Country = "France", ParentCompany = "L'Oreal", PhDistributor = "L'Oreal Philippines Inc.", OfficialWebsite = "garnier.com", BatchCodeFormat = "Alphanumeric on tube crimp or bottle bottom", BatchCodeRegex = "^[A-Z0-9]{6,10}$", BarcodePrefixes = "3600541,3600542", AuthorizedSellers = "Watson's,SM,Alfamart,Mercury Drug,Shopee Mall", AuthenticIndicators = "Garnier green leaf logo,Made in France,Dermatologically tested", CounterfeitWarnings = "Leaf logo altered,Off-brand green,Price below ₱150", PriceMin = 150, PriceMax = 800 },
            new Brand { Id = 6, Name = "Human Nature", Country = "Philippines", ParentCompany = "Gandang Kalikasan Inc.", PhDistributor = "Local Brand", OfficialWebsite = "humanheartnature.com", BatchCodeFormat = "Date-based code on label", BatchCodeRegex = "^[0-9]{6,8}$", BarcodePrefixes = "4800361", AuthorizedSellers = "Human Nature stores,SM,Robinsons,Official website", AuthenticIndicators = "Philippine Made label,PETA cruelty-free logo,Sunflower branding", CounterfeitWarnings = "Missing Philippine Made label,No PETA logo", PriceMin = 100, PriceMax = 700 },
            new Brand { Id = 7, Name = "COSRX", Country = "South Korea", ParentCompany = "COSRX Inc.", PhDistributor = "Various authorized importers", OfficialWebsite = "cosrx.com", BatchCodeFormat = "Korean + alphanumeric on box bottom", BatchCodeRegex = "^[A-Z0-9]{8,12}$", BarcodePrefixes = "8809598", AuthorizedSellers = "Althea Philippines,Shopee Mall,Lazada Mall,BeautyMNL", AuthenticIndicators = "Korean text on packaging,Holographic sticker,Made in Korea", CounterfeitWarnings = "No Korean text,Missing holographic sticker,Price below ₱400", PriceMin = 400, PriceMax = 1200 },
            new Brand { Id = 8, Name = "The Ordinary", Country = "Canada", ParentCompany = "DECIEM", PhDistributor = "Various authorized importers", OfficialWebsite = "theordinary.com", BatchCodeFormat = "Alphanumeric on tube/bottle bottom", BatchCodeRegex = "^[A-Z]{2}[0-9]{6}$", BarcodePrefixes = "655439", AuthorizedSellers = "BeautyMNL,Shopee Mall,Lazada Mall", AuthenticIndicators = "DECIEM branding,Clinical formula name,Percentage concentrations", CounterfeitWarnings = "DECIEM logo missing,Concentrations missing,Price below ₱400", PriceMin = 400, PriceMax = 900 }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Barcode = "3059500007198", Name = "Moisturizing Cream", Brand = "Cetaphil", Category = "Moisturizer", Status = "authentic", SafetyRating = "safe", Ingredients = "Water, Glycerin, Petrolatum, Dicaprylyl Ether, Dimethicone, Tocopherol" },
            new Product { Id = 2, Barcode = "3017620257537", Name = "Gentle Skin Cleanser", Brand = "Cetaphil", Category = "Cleanser", Status = "authentic", SafetyRating = "safe", Ingredients = "Water, Cetyl Alcohol, Propylene Glycol, Sodium Lauryl Sulfate, Stearyl Alcohol, Methylparaben, Propylparaben, Butylparaben" },
            new Product { Id = 3, Barcode = "3018390025543", Name = "Moisturizing Lotion with SPF 15", Brand = "CeraVe", Category = "Moisturizer + SPF", Status = "authentic", SafetyRating = "safe", Ingredients = "Water, Glycerin, Ceramide NP, Ceramide AP, Ceramide EOP, Hyaluronic Acid, Niacinamide" },
            new Product { Id = 4, Barcode = "4005808249831", Name = "Intensive Moisture Cream", Brand = "NIVEA", Category = "Body Cream", Status = "authentic", SafetyRating = "safe", Ingredients = "Water, Mineral Oil, Glycerin, Isohexadecane, Microcrystalline Wax" },
            new Product { Id = 5, Barcode = "0086800791218", Name = "Ultra Sheer Sunscreen SPF 100", Brand = "Neutrogena", Category = "Sunscreen", Status = "authentic", SafetyRating = "caution", Ingredients = "Avobenzone, Homosalate, Octisalate, Octocrylene, Oxybenzone, Water, Glycerin" }
        );
    }
}
