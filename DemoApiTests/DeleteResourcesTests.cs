using Xunit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DemoApiTests.Dto;

namespace DemoApiTests
{
    /// <summary>
    /// Tests related to the DELETE operation for the API under test.
    /// </summary>
    public class DeleteResourcesTests : DemoApiTestBase
    {
        [Fact]
        public void Delete_Existing_Post_Should_Return_Ok_Status()
        {
            // TODO: because no content is returned, this should probably return 204 NO CONTENT
            //arrange
            int id = 10;
            var request = new RestRequest($"posts/{id}", Method.DELETE);

            //act
            var response = client.Execute(request);

            //assert
           response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        
        [Fact]
        public void Delete_Invalid_Post_Should_Return_Ok_Status()
        {
            // the reasoning here is that the client wanted the resource deleted, so if it didn't exist
            // to begin with, mission accomplished.

            //arrange
            int id = 0;
            var request = new RestRequest($"posts/{id}", Method.DELETE);

            //act
            var response = client.Execute(request);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
