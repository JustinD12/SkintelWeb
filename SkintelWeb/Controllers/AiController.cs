using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace SkintelWeb.Controllers;

// ============================================================
//  Skintel AI Proxy — routes all OpenAI calls through the
//  backend so users never need their own API key.
//  The key lives ONLY in the server environment variable:
//  OPENAI_API_KEY
// ============================================================

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private static readonly HttpClient _http = new HttpClient();
    private const string OPENAI_URL = "https://api.openai.com/v1/chat/completions";
    private const string MODEL = "gpt-4o-mini";
    private const int MAX_TOKENS = 4000;

    private string? GetKey() =>
        Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    // ── Text prompt → AI response ─────────────────────────────
    [HttpPost("text")]
    public async Task<IActionResult> TextPrompt([FromBody] TextPromptRequest req)
    {
        var key = GetKey();
        if (string.IsNullOrEmpty(key))
            return StatusCode(503, new { error = "AI service not configured. Contact admin." });

        var body = new
        {
            model = MODEL,
            max_tokens = MAX_TOKENS,
            messages = new[] { new { role = "user", content = req.Prompt } }
        };

        var response = await CallOpenAI(key, body);
        return response;
    }

    // ── Image + text prompt → AI response ────────────────────
    [HttpPost("vision")]
    public async Task<IActionResult> VisionPrompt([FromBody] VisionPromptRequest req)
    {
        var key = GetKey();
        if (string.IsNullOrEmpty(key))
            return StatusCode(503, new { error = "AI service not configured. Contact admin." });

        var body = new
        {
            model = MODEL,
            max_tokens = MAX_TOKENS,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "image_url", image_url = new { url = $"data:{req.MediaType};base64,{req.Base64Image}" } },
                        new { type = "text", text = req.Prompt }
                    }
                }
            }
        };

        var response = await CallOpenAI(key, body);
        return response;
    }

    // ── Internal helper ───────────────────────────────────────
    private async Task<IActionResult> CallOpenAI(string key, object body)
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            var request = new HttpRequestMessage(HttpMethod.Post, OPENAI_URL)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {key}");

            var res = await _http.SendAsync(request);
            var raw = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, new { error = $"AI error: {res.StatusCode}" });

            // Parse and return just the message content
            using var doc = JsonDocument.Parse(raw);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Ok(new { content });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class TextPromptRequest
{
    public string Prompt { get; set; } = "";
}

public class VisionPromptRequest
{
    public string Base64Image { get; set; } = "";
    public string MediaType { get; set; } = "image/jpeg";
    public string Prompt { get; set; } = "";
}
