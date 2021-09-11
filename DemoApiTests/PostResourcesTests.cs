using Xunit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using DemoApiTests.Dto;

namespace DemoApiTests
{
    public class PostResourcesTests : DemoApiTestBase
    {
        [Fact]
        public void Post_New_Post_Should_Create_New_Post()
        {
            var body = new Dictionary<string, string>()
            {
                { "userId", "10" },
                { "title", Faker.Lorem.Sentence(4) },
                { "body", string.Join('\n', Faker.Lorem.Sentences(2)) }
            };
            var request = new RestRequest($"posts", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Data.id.Should().Be(101);
            response.Data.userId.Should().Be(int.Parse(body["userId"]));
            response.Data.title.Should().Be(body["title"]);
            response.Data.body.Should().Be(body["body"]);
        }

        [Fact]
        public void Post_Using_New_Id_Should_Create_New_Post_With_Next_Id()
        {
            var body = new Post()
            {
                id = 250,
                userId = 10,
                title = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest("posts", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            body.id = 101;
            response.Data.Should().BeEquivalentTo(body);
        }

        [Fact]
        public void Post_Using_Existing_Post_Id_Should_Create_New_Post_With_Next_Id()
        {
            var body = new Post()
            {
                id = 1,
                userId = 10,
                title = Faker.Lorem.Sentence(4),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest("posts", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            body.id = 101;
            response.Data.Should().BeEquivalentTo(body);
        }

        [Fact]
        public void Post_Without_Content_Should_Create_New_Post()
        {
            //TODO: although a new record is "created", the related data is null which is probably not ideal.

            var request = new RestRequest("posts", Method.POST);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Data.Should().BeEquivalentTo(new Post() { id = 101 });

        }

        [Fact]
        public void Post_With_Irrelevant_Body_Should_Create_New_Post()
        {
            var body = new 
            {
                Item1 = "foo",
                Item2 = "bar"
            };
            var request = new RestRequest($"posts", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Post>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Data.Should().BeEquivalentTo(new Post() { id = 101 });
        }

        [Fact]
        public void Post_Comment_Should_Create_New_Comment()
        {
            var body = new Comment()
            {
                postId = 10,
                email = Faker.Internet.Email(),
                name = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.postId}/comments", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Comment>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Data.id.Should().Be(501);
            response.Data.postId.Should().Be(body.postId);
            response.Data.email.Should().Be(body.email);
            response.Data.name.Should().Be(body.name);
            response.Data.body.Should().Be(body.body);
        }

        [Fact]
        public void Post_Comment_With_New_Comment_Id_Should_Create_Comment_With_Next_Id()
        {
            var body = new Comment()
            {
                id = 600,
                postId = 10,
                email = Faker.Internet.Email(),
                name = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.postId}/comments", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Comment>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            body.id = 501;
            response.Data.Should().BeEquivalentTo(body);
        }

        [Fact]
        public void Post_Comment_With_Existing_Comment_Id_Should_Create_Comment_With_Next_Id()
        {
            var body = new Comment()
            {
                id = 50,
                postId = 10,
                email = Faker.Internet.Email(),
                name = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.postId}/comments", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Comment>(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            body.id = 501;
            response.Data.Should().BeEquivalentTo(body);
            
        }

        [Fact]
        public void Post_Comment_With_Invalid_Post_Id_Should_Return_Bad_Request()
        {
            //TODO: creating with reference to invalid post id would probably be a bug 

            var body = new
            {
                postId = 999,
                email = Faker.Internet.Email(),
                name = Faker.Lorem.Sentence(7),
                body = string.Join('\n', Faker.Lorem.Sentences(2))
            };
            var request = new RestRequest($"posts/{body.postId}/comments", Method.POST);
            request.AddJsonBody(body);

            var response = client.Execute<Comment>(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
