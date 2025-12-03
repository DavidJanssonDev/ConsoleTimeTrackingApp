namespace TimeTracker.Domain.Entities;

/// <summary>
/// Represents a work shift associated with a project, including start and end times, notes, and duration.
/// </summary>
public class Shift
{
    /// <summary>
    /// Gets or sets the unique identifier for the shift.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated project.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the start time of the shift.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shift, if available.
    /// </summary>
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// Gets or sets an optional note for the shift.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets the associated project.
    /// </summary>
    public Project Project { get; set; } = null!; // Navigation property

    /// <summary>
    /// Gets the duration of the shift. Returns zero if the shift is open.
    /// </summary>
    public TimeSpan Duration =>
        EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;

    /// <summary>
    /// Gets a value indicating whether the shift is currently open (not ended).
    /// </summary>
    public bool IsOpen => EndTime == null;
}