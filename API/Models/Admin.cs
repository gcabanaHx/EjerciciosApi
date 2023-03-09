using System.Text.Json.Serialization;

namespace API.Models
{
    public class Admin
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string? UserName { get; set; }

        [JsonPropertyName("passHash")]
        public string? PassHash { get; set; }
    }
}