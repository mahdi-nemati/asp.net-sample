using Microsoft.AspNetCore.Mvc;

namespace Endpoint.RequestsAndResponses
{
    public class TeacherRequest
    {

        [FromForm]
        public string FirstName { get; set; }
        [FromForm]
        public string LastName { get; set; }
        [FromForm]
        public DateTime Birthday { get; set; }
        [FromForm]
        public IFormFile? File { get; set; }
    }

    public class TeacherResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string ImageUrl { get; set; }

    }
}
