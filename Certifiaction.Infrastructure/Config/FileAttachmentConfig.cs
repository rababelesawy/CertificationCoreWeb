using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certification.Infrastructure.Config
{
    public class FileAttachmentConfig : IEntityTypeConfiguration<FileAttachment>
    {
        public void Configure(EntityTypeBuilder<FileAttachment> builder)
        {
            builder.ToTable("FileAttachment");
            builder.HasKey(f => f.FileAttachmentId);
        }
    }
}
