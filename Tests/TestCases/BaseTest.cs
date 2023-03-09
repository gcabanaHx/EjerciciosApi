using API.REST;
using Tests.Helpers;

namespace Tests.TestCases
{
    public class BaseTest
    {
        protected readonly RestSharpClient client;

        public BaseTest()
        {
            client = RestClientManager.Client;
        }
    }
}