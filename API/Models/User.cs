using System.Text.Json.Serialization;

namespace API.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }


        [JsonPropertyName("phoneNumber")]
        public int? PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}