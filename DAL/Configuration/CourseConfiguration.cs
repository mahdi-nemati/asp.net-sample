using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        void IEntityTypeConfiguration<Course>.Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(p => p.Tilte).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.ImageUrl).IsRequired().HasMaxLength(200).IsUnicode(false);
        }
    }
}
