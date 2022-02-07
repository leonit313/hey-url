using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Map
{
    public class UrlClickMap : IEntityTypeConfiguration<UrlClickEntity>
    {
        public void Configure(EntityTypeBuilder<UrlClickEntity> builder)
        {
            builder.ToTable("UrlClick");

            builder.HasKey(c => c.Id);

            builder.HasOne(t => t.Url).WithMany().HasForeignKey(t => t.UrlId);

            builder.Property(c => c.Date).IsRequired();

            builder.Property(c => c.Platform).IsRequired();

            builder.Property(c => c.Browser).IsRequired();

        }
    }
}
