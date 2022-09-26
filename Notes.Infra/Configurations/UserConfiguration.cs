using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Infra.Seedings;

namespace Notes.Infra.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private readonly ISeeder _seeder;

    public UserConfiguration(ISeeder seeder) => _seeder = seeder;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        _seeder.SeedUser(builder);
       
        builder
            .HasIndex(u => u.Email)
            .IsUnique();

        builder
            .HasIndex(u => u.AppUserId)
            .IsUnique();
    }
}
