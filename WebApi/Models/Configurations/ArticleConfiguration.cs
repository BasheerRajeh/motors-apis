using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> entity)
        {
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.SubTitle).HasMaxLength(300);
            entity.Property(e => e.ParagraphOne).HasMaxLength(4000);
            entity.Property(e => e.ParagraphTwo).HasMaxLength(4000);
            entity.Property(e => e.Language).HasMaxLength(20);
            entity.Property(e => e.ImagePath).HasMaxLength(150);
        }
    }
    public class ArticleImageConfiguration : IEntityTypeConfiguration<ArticleImage>
    {
        public void Configure(EntityTypeBuilder<ArticleImage> entity)
        {
            entity.Property(e => e.Path).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.MimeType).HasMaxLength(100);
            entity.ToTable("Article_Images");
        }
    }
}
