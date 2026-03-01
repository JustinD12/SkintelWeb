using Microsoft.EntityFrameworkCore;
using SkintelWeb.Data;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Environment.GetEnvironmentVariable("SKINTEL_DB_PATH") ?? "skintel.db";

builder.Services.AddDbContext<SkintelDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

// Allow mobile app to connect from any origin
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Auto-create and seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SkintelDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();

// ── Admin-only middleware ──────────────────────────────────────────────────────
// GET requests are PUBLIC — mobile app can read products, brands, ingredients.
// POST / PUT / DELETE require X-Admin-Key header (set SKINTEL_ADMIN_KEY env var).
// /api/ai is fully public — the OpenAI key lives in OPENAI_API_KEY env var.
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    var method = context.Request.Method;

    bool isAdminRoute =
        path.StartsWith("/api/brands") ||
        path.StartsWith("/api/products") ||
        path.StartsWith("/api/ingredients");

    bool isWriteOperation = method == "POST" || method == "PUT" || method == "DELETE";

    if (isAdminRoute && isWriteOperation)
    {
        var adminKey = Environment.GetEnvironmentVariable("SKINTEL_ADMIN_KEY") ?? "skintel-admin-2025";
        var providedKey = context.Request.Headers["X-Admin-Key"].FirstOrDefault();

        if (providedKey != adminKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized. Admin key required." });
            return;
        }
    }

    await next();
});

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
