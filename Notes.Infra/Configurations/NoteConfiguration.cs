using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Infra.Data;

namespace Notes.Infra.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasQueryFilter(n => n.IsDeleted == false);

        var notes = SeedHelper.SeedData<Note>("Notes.json");

        if (notes == null) return;

        builder.HasData(notes);

    }
}
