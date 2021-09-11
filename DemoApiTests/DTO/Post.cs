


namespace DemoApiTests.Dto
{
    public record Post
    {
        public int? id { get; set; }
        public int? userId { get; set; }
        public string title { get; set; }
        public string body { get; set; }

    }
}