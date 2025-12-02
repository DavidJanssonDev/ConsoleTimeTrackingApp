namespace TimeTracker.Infrastructre.Persistence;

/// <summary>
/// Provides methods for managing <see cref="Shift"/> entities in the database.
/// </summary>
public class ShiftRepository(TimeTrackerDbContext db) : IShiftRepository
{
    private readonly TimeTrackerDbContext _db = db;

    /// <inheritdoc/>
    public async Task<Shift> AddAsync(Shift shift)
    {
        _db.Shifts.Add(shift);
        await _db.SaveChangesAsync();
        await _db.Entry(shift).Reference(s => s.Project).LoadAsync();
        return shift;
    }
    /// <inheritdoc/>
    public Task<List<Shift>> GetAllAsync() => _db.Shifts.Include(shift => shift.Project).OrderByDescending(shift => shift.StartTime).ToListAsync();

    /// <inheritdoc/>
    public Task<Shift?> GetByIdAsync(long id) => _db.Shifts.Include(s => s.Project).FirstOrDefaultAsync(s => s.Id == id);

    /// <inheritdoc/>
    public async Task EndAsync(long id, DateTimeOffset endTime)
    {
        Shift? shift = await GetByIdAsync(id) ?? throw new Exception("Shift not found");
        if (shift.EndTime.HasValue) throw new Exception("Already ended");
        if (endTime <= shift.StartTime) throw new Exception("End must be after start");
        shift.EndTime = endTime;
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(long id)
    {
        Shift? shift = await GetByIdAsync(id);
        if (shift != null)
        {
            _db.Shifts.Remove(shift);
            await _db.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<Dictionary<string, TimeSpan>> TotalsByProjectAsync()
    {
        var result = await _db.Shifts
            .Where(shift => shift.EndTime != null)
            .GroupBy(shift => shift.Project.Name)
            .Select(group => new { ProjectName = group.Key, TotalTicks = group.Sum(s => (s.EndTime!.Value - s.StartTime).Ticks) })
            .ToListAsync();

        // Convert to Dictionary<string, TimeSpan>
        return result.ToDictionary(x => x.ProjectName, x => TimeSpan.FromTicks(x.TotalTicks));

    }
}
