using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notes.Infra.Seedings;

public class DevSeeder : ISeeder
{
    public void SeedNotes(EntityTypeBuilder<Note> builder)
    {
        var notes = SeedHelper.SeedData<Note>("Notes.json");

        if (notes == null) return;

        builder.HasData(notes);
    }

    public void SeedUser(EntityTypeBuilder<User> builder)
    {
        builder.HasData(new User
        {
            Id = 1,
            AppUserId = "TestAppUserId",
            Email = "test@test.com",
            FirstName = "Tester",
            LastName = "Tester"
        });

    }
}
