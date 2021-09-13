using Xunit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DemoApiTests.Dto;

namespace DemoApiTests
{
    public class PutResourcesTest : DemoApiTestBase
    {
        [Fact]
        public void Put_Update_Post_With_Id_In_Body_Should_Update_Post()
        {
            //TODO: what if the route Id != body Id...

            var body = new Post()
            {
                id = 5,
                userId = 10,
                title = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.id}", Method.PUT);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().BeEquivalentTo(body);
        }

        [Fact]
        public void Put_Update_Post_Without_Id_In_Body_Should_Update_Post()
        {
            var id = 8;
            var body = new 
            {
                userId = 10,
                title = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{id}", Method.PUT);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.id.Should().Be(id);
            response.Data.userId.Should().Be(body.userId);
            response.Data.title.Should().Be(body.title);
            response.Data.body.Should().Be(body.body);
        }

        [Fact]
        public void Put_Update_Post_Partial_Data_Should_Null_Missing_Items()
        {
            var id = 8;
            var body = new 
            {
                title = Faker.Lorem.Sentence(7)
            };
            var request = new RestRequest($"posts/{id}", Method.PUT);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.id.Should().Be(id);
            response.Data.userId.Should().BeNull();
            response.Data.title.Should().Be(body.title);
            response.Data.body.Should().BeNull();
        }

        [Fact]
        public void Put_Update_Post_Using_Invalid_Post_Id_Should_Return_Error()
        {
            //TODO: should probably return 404 not found or create the post and return 201.

            var body = new Post()
            {
                id = 110,
                userId = 1,
                title = Faker.Lorem.Sentence(4),
                body = string.Join('\n', Faker.Lorem.Sentences(1))
            };
            var request = new RestRequest($"posts/{body.id}", Method.PUT);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Put_Update_Post_Using_Invalid_User_Id_Should_Return_Error()
        {
            //TODO: should probably return bad request given the invalid user id

            var body = new Post()
            {
                id = 5,
                userId = 15,
                title = Faker.Lorem.Sentence(6),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.id}", Method.PUT);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }

        [Theory]
        [InlineData("Content-Type", "application/json; charset=utf-8")]
        [InlineData("Connection", "keep-alive")]
        [InlineData("Cache-Control", "no-cache")]
        public void Put_Posts_Response_Header_Items(string header, string value)
        {
            //arrange
            var body = new Post()
            {
                id = 5,
                userId = 10,
                title = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.id}", Method.PUT);
            request.AddJsonBody(body);

            //act
            var response = client.Execute<Post>(request);
            var headerValue = response.Headers
                .Where(x => x.Name == header)
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            //assert
            headerValue.Should().Be(value);

        }
    }


}
