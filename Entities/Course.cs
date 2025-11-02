namespace Entities
{
    public sealed class Course : BaseEntity
    {
        public string Tilte { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOnline { get; set; }
        public string ImageUrl { get; set; }
    }
}
