using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Map
{
    public class UrlMap : IEntityTypeConfiguration<UrlEntity>
    {
        public void Configure(EntityTypeBuilder<UrlEntity> builder)
        {
            builder.ToTable("Url");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Date).IsRequired();

            builder.Property(c => c.ShortUrl).IsRequired();

            builder.Property(c => c.OriginalUrl).IsRequired();

        }
    }
}
