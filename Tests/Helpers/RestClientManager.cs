using API.REST;
using Microsoft.Extensions.Configuration;

namespace Tests.Helpers
{
    public static class RestClientManager
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public static RestSharpClient Client { get; } = new RestSharpClient(Configuration["BaseUrl"]);
    }
}