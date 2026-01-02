namespace TimeTracker.Infrastructre.Persistence;

/// <summary>
/// Provides methods for managing <see cref="Shift"/> entities in the database.
/// </summary>
public class EfShiftRepository(TimeTrackerDbContext db) : IShiftRepository
{
    private readonly TimeTrackerDbContext _db = db;

    /// <inheritdoc/>
    public async Task<Shift> AddAsync(Shift shift)
    {
        _db.Shifts.Add(shift);
        await _db.SaveChangesAsync();

        // Ensure navigation is available for UI formatting
        await _db.Entry(shift).Reference(s => s.Project).LoadAsync();
        return shift;
    }

    public async Task<Shift?> GetByIdAsync(long id)
    {
        return await _db.Shifts
            .Include(s => s.Project)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Shift>> GetAllAsync()
    {
        return await _db.Shifts
            .Include(s => s.Project)
            .OrderByDescending(s => s.StartUtc)
            .ToListAsync();
    }

    public async Task EndAsync(long id, DateTimeOffset endTime)
    {
        Shift? shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        if (shift is null)
        {
            throw new InvalidOperationException("Shift not found.");
        }

        if (shift.EndUtc is not null)
        {
            // already ended
            return;
        }

        shift.EndTime = endTime; // writes EndUtc via NotMapped wrapper
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        Shift? shift = await _db.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        if (shift is null)
        {
            return;
        }

        _db.Shifts.Remove(shift);
        await _db.SaveChangesAsync();
    }

    public async Task<Dictionary<string, TimeSpan>> TotalsByProjectAsync()
    {
        List<Shift> shifts = await _db.Shifts
            .Include(s => s.Project)
            .ToListAsync();

        Dictionary<string, TimeSpan> totals = new(StringComparer.Ordinal);

        for (int i = 0; i < shifts.Count; i++)
        {
            Shift s = shifts[i];
            if (s.EndUtc is null)
            {
                continue; // don’t count open shift
            }

            string key = s.Project.Name;
            if (!totals.TryGetValue(key, out TimeSpan current))
            {
                current = TimeSpan.Zero;
            }

            totals[key] = current + s.Duration;
        }

        return totals;
    }
}
