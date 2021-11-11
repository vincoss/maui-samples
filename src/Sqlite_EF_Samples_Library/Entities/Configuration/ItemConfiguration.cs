using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sqlite_EF_Samples_Library.Entities.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item))
                   .HasIndex(x => new { x.ItemId }).IsUnique();

            builder.HasKey(x => x.ItemId);

            builder.Property(t => t.Name)
                .IsRequired();

            builder.Property(t => t.Description);

            builder.Property(t => t.JobId);

            builder.Property(t => t.Status);
            builder.Property(t => t.Priority);
            builder.Property(t => t.Assigne);
            builder.Property(t => t.Tags);

            builder.Property(x => x.DueDateUtc);

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired();

            builder.Property(x => x.ModifiedDateTimeUtc)
                  .IsRequired();

            builder.Property(x => x.RowUuid)
                   .IsRequired();
        }
    }
}

