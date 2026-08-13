namespace CleanArchWithVerticalSliceArchTemplate.Domain.Entities
{
    public class AuditableEntity
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}