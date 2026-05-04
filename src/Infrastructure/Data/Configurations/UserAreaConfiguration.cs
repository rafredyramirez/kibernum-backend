using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserAreaConfiguration : IEntityTypeConfiguration<UserArea>
    {
        public void Configure(EntityTypeBuilder<UserArea> builder)
        {
            builder.ToTable("UserArea");

            builder.HasKey(ua => ua.UserId);

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<UserArea>(ua => ua.UserId);

            builder.HasOne<Area>()
                .WithMany()
                .HasForeignKey(ua => ua.AreaId);
        }
    }
}
