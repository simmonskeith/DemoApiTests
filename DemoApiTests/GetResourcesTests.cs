using Xunit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using DemoApiTests.Dto;


namespace DemoApiTests
{

    /// <summary>
    /// Tests related to the GET operation for the API under test.
    /// </summary>
    public class GetResourcesTests : DemoApiTestBase
    {
        
        [Fact]
        public void Get_All_Posts_Should_Return_All_Posts()
        {
            //arrange
            // we know the count of posts for the test api, but it would be better to pull the count from our data source
            var expectedItems = 100;  
            var request = new RestRequest("posts", Method.GET);

            //act
            var response = client.Execute<List<Post>>(request);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Count().Should().Be(expectedItems);
        }

        [Theory]
        [InlineData("Content-Type", "application/json; charset=utf-8")]
        [InlineData("Connection", "keep-alive")]
        public void Get_Posts_Response_Header_Items(string header, string value)
        {
            //arrange
            var request = new RestRequest("posts", Method.GET);

            //act
            var response = client.Execute<List<Post>>(request);
            var headerValue = response.Headers
                .Where(x => x.Name == header)
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            //assert
            headerValue.Should().Be(value);
           
        }

        [Theory]
        [InlineData(1, "sunt aut facere repellat provident occaecati excepturi optio reprehenderit")]
        [InlineData(25, "rem alias distinctio quo quis")]
        [InlineData(100, "at nam consequatur ea labore ea harum")]
        public void Get_Post_By_Id_Should_Return_Requested_Post(int id, string title)
        {
            //arrange
            var request = new RestRequest($"posts/{id}");

            //act
            var response = client.Execute<Post>(request);

            //assert
            response.Data.title.Should().Be(title);
        }

        [Fact]
        public void Get_Post_By_Invalid_Post_Id_Should_Return_Not_Found()
        {
            //arrange
            int id = 0;
            var request = new RestRequest($"posts/{id}", Method.GET);

            //act
            var response = client.Execute<Post>(request);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1, new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(7, new int[] { 31, 32, 33, 34, 35})]
        public void Get_Comments_By_Post_Id_Should_Return_Comments_For_Post(int postId, int[] ids)
        {
            var request = new RestRequest($"comments?postId={postId}", Method.GET);

            var response = client.Execute<List<Comment>>(request);
            var responseIds = response.Data.OrderBy(x => x.id).Select(x => x.id).ToArray();

            responseIds.Should().BeEquivalentTo(ids);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("501")]
        [InlineData("foo")]
        public void Get_Comments_By_Invalid_Post_Id_Should_Not_Return_Comments(string postId)
        {
            var request = new RestRequest($"comments?postId={postId}", Method.GET);

            var response = client.Execute<List<Comment>>(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Count().Should().Be(0);
        }
    }
}
