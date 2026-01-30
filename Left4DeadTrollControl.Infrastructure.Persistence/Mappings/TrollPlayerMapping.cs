namespace Left4DeadTrollControl.Infrastructure.Persistence.Mappings;

public class TrollPlayerMapping : IEntityTypeConfiguration<TrollPlayer>
{
    public void Configure(EntityTypeBuilder<TrollPlayer> builder)
    {
        builder
            .ToTable("TrollPlayer")
            .HasKey(tp => tp.Id);

        builder
            .Property(tp => tp.SteamId)
            .IsRequired()
            .HasMaxLength(8);

        builder
            .Property(tp => tp.ProfileUrl)
            .IsRequired(false)
            .HasMaxLength(300);

        builder
            .Property(tp => tp.Nickname)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(tp => tp.Notes)
            .IsRequired()
            .HasMaxLength(2000);

        builder
            .Property(tp => tp.CreatedAt)
            .IsRequired();

        builder
            .Property(tp => tp.UpdatedAt)
            .IsRequired(false);
    }
}
