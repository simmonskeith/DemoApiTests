

using Microsoft.Extensions.Configuration;
using RestSharp;

namespace DemoApiTests
{
    public class DemoApiTestBase
    {
        protected IConfigurationRoot config;
        protected RestClient client;

        /// <summary>
        /// Base class for API tests.  Loads test configuration and creates the client used throughout 
        /// the various API tests.
        /// </summary>
        public DemoApiTestBase()
        {
            config = new ConfigurationBuilder().AddJsonFile("test-config.json").Build();
            client = new RestClient(config["BASE_URL"]);
        }
    }
}