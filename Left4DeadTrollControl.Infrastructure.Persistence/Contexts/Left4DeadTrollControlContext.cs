namespace Left4DeadTrollControl.Infrastructure.Persistence.Contexts;

public class Left4DeadTrollControlContext : DbContext
{
    private static int _instanceCount = 0;
    private readonly int _instanceId;

    public DbSet<TrollPlayer> Trolls { get; set; }

    public Left4DeadTrollControlContext(DbContextOptions<Left4DeadTrollControlContext> options) : base(options) 
    {
        _instanceId = System.Threading.Interlocked.Increment(ref _instanceCount);
        System.Diagnostics.Debug.WriteLine($"[DbContext #{_instanceId}] CRIADO");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override void Dispose()
    {
        System.Diagnostics.Debug.WriteLine($"[DbContext #{_instanceId}] DESCARTADO");
        base.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        System.Diagnostics.Debug.WriteLine($"[DbContext #{_instanceId}] DESCARTADO (Async)");
        await base.DisposeAsync();
    }
}
