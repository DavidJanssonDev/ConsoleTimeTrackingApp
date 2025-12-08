namespace TimeTracker.Domain.Interfaces;

/// <summary>
/// Provides methods for managing work shifts, including starting and ending shifts.
/// </summary>
public interface IShiftService
{
    /// <summary>
    /// Starts a new shift for the specified project.
    /// </summary>
    /// <param name="projectName">The name of the project for which the shift is started.</param>
    /// <param name="note">An optional note describing the shift.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the started <see cref="Shift"/>.</returns>
    Task<Shift> StartShiftAsync(string projectName, string? note);

    /// <summary>
    /// Ends the shift with the specified identifier.
    /// </summary>
    /// <param name="shiftId">The identifier of the shift to end.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EndShiftAsync(long shiftId);
}

