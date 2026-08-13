namespace CleanArchWithVerticalSliceArchTemplate.Domain.Entities
{
    public class Book : AuditableEntity
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public decimal Price { get; set; }
        public int PublishedYear { get; set; }
    }
}