using Microsoft.Extensions.Configuration;
using Xunit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DemoApiTests.Dto;


namespace DemoApiTests
{


    public class GetResourcesTests : DemoApiTestBase
    {
        
        [Fact]
        public void GetAllResourcesShouldReturnOkStatus()
        {
            //arrange
            var request = new RestRequest("posts");

            //act
            var response = client.Execute<List<Resource>>(request);

            //assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);   
        }

        [Theory]
        [InlineData("Content-Type", "application/json; charset=utf-8")]
        [InlineData("Connection", "keep-alive")]
        public void GetAllResourcesHeadersTest(string header, string value)
        {
            //arrange
            var request = new RestRequest("posts");

            //act
            var response = client.Execute<List<Resource>>(request);
            var headerValue = response.Headers
                .Where(x => x.Name == header)
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            //assert
            headerValue.Should().Be(value);
           
        }

        [Fact]
        public void GetAllResourcesShouldReturnResources()
        {
            //arrange
            var request = new RestRequest("posts");

            //act
            var response = client.Execute<List<Resource>>(request);

            //assert
            response.Data.Count().Should().BeGreaterOrEqualTo(0);
        }

        [Theory]
        [InlineData("1", "sunt aut facere repellat provident occaecati excepturi optio reprehenderit")]
        [InlineData("25", "rem alias distinctio quo quis")]
        [InlineData("100", "at nam consequatur ea labore ea harum")]
        public void GetResourceShouldReturnRequestedResource(string id, string title)
        {
            //arrange
            var request = new RestRequest($"posts/{id}");

            //act
            var response = client.Execute<Resource>(request);

            //assert
            response.Data.Title.Should().Be(title);
        }

        [Fact]
        public void GetInvalidResourceShouldReturnNotFound()
        {
            //arrange
            var request = new RestRequest($"posts/0");

            //act
            var response = client.Execute<Resource>(request);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
