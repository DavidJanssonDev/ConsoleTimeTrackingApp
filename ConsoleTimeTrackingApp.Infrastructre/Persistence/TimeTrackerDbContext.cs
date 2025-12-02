namespace TimeTracker.Infrastructre.Persistence;

/// <summary>
/// Database helper: creates DB file + tables if they don't exist
/// </summary>
public class TimeTrackerDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Gets the set of projects in the database.
    /// </summary>
    public DbSet<Project> Projects => Set<Project>();

    /// <summary>
    /// Gets the set of shifts in the database.
    /// </summary>
    public DbSet<Shift> Shifts => Set<Shift>();
}

