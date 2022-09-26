using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Infra.Seedings;

namespace Notes.Infra.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    private readonly ISeeder _seeder;

    public NoteConfiguration(ISeeder seeder) => _seeder = seeder;

    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasQueryFilter(n => n.IsDeleted == false);

        _seeder.SeedNotes(builder);
    }
}
