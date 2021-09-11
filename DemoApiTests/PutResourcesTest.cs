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
        public void PutUpdatePostIncludingIdInBodyShouldUpdatePost()
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
        public void PutUpdatePostWithoutIdInBodyShouldReturnUpdatePost()
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
        public void PutUpdatePostPartialDataShouldInsertNullsInMissingItems()
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
        public void PutUpdatePostUsingInvalidPostIdShouldReturnError()
        {
            //TODO: should probably return 404 not found given the non-existing post id

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
        public void PutUpdatePostUsingInvalidUserIdShouldReturnError()
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
    }


}
