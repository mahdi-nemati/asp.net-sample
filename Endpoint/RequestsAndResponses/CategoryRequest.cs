namespace Endpoint.RequestsAndResponses
{
    public class CategoryRequest
    {
        public string Name { get; set; }
    }

    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
