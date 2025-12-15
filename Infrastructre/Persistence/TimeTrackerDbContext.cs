namespace TimeTracker.Infrastructre.Persistence;

/// <summary>
/// EF Core DbContext for the application.
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

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {

            entity.HasKey(project => project.Id);
            entity.Property(project => project.Name).IsRequired();

            entity.HasMany(p => p.Shifts)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(shift => shift.Id);

            // Persisted Fields are StartUtc + EndUtc (see Shfit.cs)
            entity.Property(shift => shift.StartUtc).IsRequired();
            entity.Property(shift => shift.EndUtc).IsRequired(false);

            entity.Property(shift => shift.Note).IsRequired(false);
        });
    }
}

