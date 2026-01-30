namespace Left4DeadTrollControl.Infrastructure.Persistence.Contexts;

public class Left4DeadTrollControlContextFactory : IDesignTimeDbContextFactory<Left4DeadTrollControlContext>
{
    public Left4DeadTrollControlContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Left4DeadTrollControlContext>();

        // Durante migrations, usa um caminho relativo ao projeto de infraestrutura
        var projectPath = Directory.GetCurrentDirectory();
        var dbPath = Path.Combine(projectPath, "l4dtc.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new Left4DeadTrollControlContext(optionsBuilder.Options);
    }
}