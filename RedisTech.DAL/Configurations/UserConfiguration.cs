using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedisTech.Domain.Entity;

namespace RedisTech.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Login).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Password).IsRequired();
        
        builder.HasData(new List<User>()
        {
            new User()
            {
                Id = 1,
                Login = "ITHomester",
                Password = new string('-', 20),
                CreatedAt = DateTime.UtcNow
            }
        });
    }
}