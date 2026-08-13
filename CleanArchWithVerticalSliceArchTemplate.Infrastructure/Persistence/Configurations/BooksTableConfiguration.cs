namespace CleanArchWithVerticalSliceArchTemplate.Infrastructure.Persistence.Configurations
{
    using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BooksTableConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("books");

            // Id:
            builder.HasKey((book) => book.Id);

            builder
                .Property((book) => book.Id)
                .HasColumnName("book_id")
                .IsRequired();

            // Other fields:
            builder
                .Property((book) => book.Title)
                .HasColumnName("title")
                .HasMaxLength(350)
                .IsRequired();

            builder
                .Property((book) => book.Author)
                .HasColumnName("author")
                .HasMaxLength(350)
                .IsRequired();

            builder
                .Property((book) => book.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            builder
                .Property((book) => book.ISBN)
                .HasColumnName("isbn")
                .HasPrecision(350)
                .IsRequired();

            builder
                .Property((book) => book.PublishedYear)
                .HasColumnName("published_year")
                .HasDefaultValue(DateTime.UtcNow.Year)
                .IsRequired();

            // Dates:
            builder
                .Property((book) => book.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            builder
                .Property((book) => book.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()")
                .IsRequired();
        }
    }
}