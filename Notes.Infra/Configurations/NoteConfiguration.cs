using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notes.Infra.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{

    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasQueryFilter(n => n.IsDeleted == false);
    }
}
