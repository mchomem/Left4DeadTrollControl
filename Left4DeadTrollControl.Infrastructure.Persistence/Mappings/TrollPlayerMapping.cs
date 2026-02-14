namespace Left4DeadTrollControl.Infrastructure.Persistence.Mappings;

public class TrollPlayerMapping : IEntityTypeConfiguration<TrollPlayer>
{
    public void Configure(EntityTypeBuilder<TrollPlayer> builder)
    {
        builder
            .ToTable("TrollPlayer")
            .HasKey(tp => tp.Id);

        // CRÍTICO: Configuração explícita de conversão Guid <-> string para SQLite
        builder
            .Property(tp => tp.Id)
            .HasConversion(
                v => v.ToString().ToLower(),  // Guid -> string (sempre lowercase)
                v => Guid.Parse(v)            // string -> Guid
            )
            .HasColumnType("TEXT");

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
