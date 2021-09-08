

using Microsoft.Extensions.Configuration;
using RestSharp;

namespace DemoApiTests
{
    public class DemoApiTestBase
    {
        protected IConfigurationRoot config;
        protected RestClient client;

        public DemoApiTestBase()
        {
            config = new ConfigurationBuilder().AddJsonFile("test-config.json").Build();
            client = new RestClient(config["BASE_URL"]);
        }
    }
}