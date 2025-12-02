namespace TimeTracker.Domain.Interfaces;

/// <summary>
/// Provides methods for managing <see cref="Shift"/> entities, including creation, retrieval, update, deletion,
/// and calculation of total time spent per project.
/// </summary>
public interface IShiftRepository
{
    /// <summary>
    /// Adds a new shift asynchronously.
    /// </summary>
    /// <param name="shift">The shift to add.</param>
    /// <returns>The added <see cref="Shift"/>.</returns>
    Task<Shift> AddAsync(Shift shift);

    /// <summary>
    /// Gets a shift by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The shift identifier.</param>
    /// <returns>The <see cref="Shift"/> if found; otherwise, null.</returns>
    Task<Shift?> GetByIdAsync(long id);

    /// <summary>
    /// Gets all shifts asynchronously.
    /// </summary>
    /// <returns>A list of all <see cref="Shift"/> entities.</returns>
    Task<List<Shift>> GetAllAsync();

    /// <summary>
    /// Ends a shift by setting its end time asynchronously.
    /// </summary>
    /// <param name="id">The shift identifier.</param>
    /// <param name="endTime">The end time to set.</param>
    Task EndAsync(long id, DateTimeOffset endTime);

    /// <summary>
    /// Deletes a shift asynchronously.
    /// </summary>
    /// <param name="id">The shift identifier.</param>
    Task DeleteAsync(long id);

    /// <summary>
    /// Calculates total time spent per project asynchronously.
    /// </summary>
    /// <returns>
    /// A dictionary mapping project names to total time spent (<see cref="TimeSpan"/>).
    /// </returns>
    Task<Dictionary<string, TimeSpan>> TotalsByProjectAsync();
}

