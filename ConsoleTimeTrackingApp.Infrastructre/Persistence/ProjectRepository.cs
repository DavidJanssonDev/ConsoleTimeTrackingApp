namespace TimeTracker.Infrastructre.Persistence;

/// <summary>
/// Provides methods for managing <see cref="Project"/> entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProjectRepository"/> class.
/// </remarks>
/// <param name="db">The database context.</param>
public class ProjectRepository(TimeTrackerDbContext db) : IProjectRepository
{
    private readonly TimeTrackerDbContext _db = db;

    /// <inheritdoc />
    public async Task<Project> CreateAsync(string name)
    {
        name = name.Trim();
        Project? existing = await _db.Projects.FirstOrDefaultAsync(p => p.Name == name);
        if (existing != null) return existing;
        
        Project project = new() { Name = name };
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();
        return project;
    }

    /// <inheritdoc />
    public Task<List<Project>> GetAllAsync() => _db.Projects.OrderBy(p => p.Name).ToListAsync();

    /// <inheritdoc />
    public Task<Project?> GetByIdAsync(int id) => _db.Projects.Include(p => p.Shifts).FirstOrDefaultAsync(p => p.Id == id);

    /// <inheritdoc />
    public Task<Project?> GetByNameAsync(string name) => _db.Projects.FirstOrDefaultAsync(p => p.Name == name.Trim());
}