using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notes.Infra.Seedings;

public interface ISeeder
{
    void SeedNotes(EntityTypeBuilder<Note> builder);
    void SeedUser(EntityTypeBuilder<User> builder);
}
