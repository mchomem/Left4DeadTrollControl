namespace Left4DeadTrollControl.Infrastructure.Persistence.Contexts;

public class Left4DeadTrollControlContext : DbContext
{
    public DbSet<TrollPlayer> Trolls { get; set; }

    public Left4DeadTrollControlContext(DbContextOptions<Left4DeadTrollControlContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
