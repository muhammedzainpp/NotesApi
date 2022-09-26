using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Infra.Seedings;

namespace Notes.IntegrationTests.Mocks;

public class DummySeeder : ISeeder
{
    public void SeedNotes(EntityTypeBuilder<Note> builder)
    {
    }

    public void SeedUser(EntityTypeBuilder<User> builder)
    {
    }
}
