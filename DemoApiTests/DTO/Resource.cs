


namespace DemoApiTests.Dto
{
    public record Resource
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

    }
}