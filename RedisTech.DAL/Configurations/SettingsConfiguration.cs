using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedisTech.Domain.Entity;

namespace RedisTech.DAL.Configurations;

public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired();
        
        builder.HasData(new List<Settings>()
        {
            new Settings()
            {
                Id = 1,
                Name = "Email",
                Description = "lewodQWJJE@bk.ru",
            },
            new Settings()
            {
                Id = 2,
                Name = "Password",
                Description = "daszdaskEUJWAJEWAEWA@@#--32#@sda",
            }
        });
    }
}