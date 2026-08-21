using System.Text;
using System.Text.Json;

namespace EmergencyResponse.Api.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"]
                ?? throw new Exception("Gemini API key is not configured.");
        }

        public async Task<string> AnalyzeIncidentTextAsync(string rawText)
        {
            var prompt = $@"
Si asistent pre dispečera záchranných zložiek. Analyzuj nasledujúci text hlásenia a vráť ČISTO JSON (žiadny iný text, žiadne markdown značky) s týmito poľami:
- type: typ incidentu (napr. ""Dopravná nehoda"", ""Požiar"", ""Zdravotný problém"")
- description: krátke zhrnutie situácie (1-2 vety)
- urgency: jedno z ""Low"", ""Medium"", ""High"", ""Critical""
- suggestedUnits: zoznam odporúčaných jednotiek (napr. [""Ambulance"", ""Police""])
Text hlásenia: ""{rawText}""
";
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.6-flash:generateContent?key={_apiKey}";
            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API error: {responseBody}");
            }
            using var doc = JsonDocument.Parse(responseBody);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
            return text ?? string.Empty;
        }
    }
}