namespace Entities
{
    public sealed class Teacher : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string ImageUrl { get; set; }
    }
}
