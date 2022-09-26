using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notes.Infra.Seedings;

public class MockSeeder : ISeeder
{
    public void SeedNotes(EntityTypeBuilder<Note> builder)
    {
    }

    public void SeedUser(EntityTypeBuilder<User> builder)
    {
    }
}
