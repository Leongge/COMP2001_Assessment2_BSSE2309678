using COMP2001_Assessment2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace COMP2001_Assessment2.Controllers
{
    public class UserTrail : Controller
    {
        private readonly ILogger<UpdateProfile> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public UserTrail(ILogger<UpdateProfile> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var trails = await GetUserTrails(userId.Value);
            return View(trails);
        }

        private async Task<List<Trail>> GetUserTrails(int userId)
        {
            var apiUrl = $"https://localhost:7037/api/Trails";
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringDateTimeConverter("TrailStartDate"),
                        }
                };
                var trails = JsonSerializer.Deserialize<List<Trail>>(content, jsonOptions);
                _logger.LogInformation($"FollowRelationships data: {JsonSerializer.Serialize(trails)}");

                var userTrails = trails.Where(a => a.ProfileId == userId).ToList();

                return userTrails;
            }

            return new List<Trail>();
        }

        public class JsonStringDateTimeConverter : JsonConverter<DateTime>
        {
            private readonly string _propertyName;

            public JsonStringDateTimeConverter(string propertyName)
            {
                _propertyName = propertyName;
            }

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.GetString() != null && reader.GetString() != string.Empty)
                {
                    if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    {
                        return date;
                    }
                }

                return default;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
            }
        }
    }

}
