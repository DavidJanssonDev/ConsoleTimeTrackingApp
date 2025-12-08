using System.ComponentModel.DataAnnotations.Schema;

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

    // ---------------------------
    // Persisted fields (SQLite supports DateTime)
    // ---------------------------

    /// <summary>
    /// UTC start time stored in the database.
    /// </summary>
    public DateTime StartUtc { get; set; }

    /// <summary>
    /// UTC end time stored in the database. Null means shift is still open.
    /// </summary>
    public DateTime? EndUtc { get; set; }

    // ---------------------------
    // Convenience wrappers (NOT persisted)
    // ---------------------------

    /// <summary>
    /// Start time exposed as DateTimeOffset for your app logic/UI.
    /// Not mapped because SQLite can't reliably order/compare DateTimeOffset.
    /// </summary>
    [NotMapped]
    public DateTimeOffset StartTime
    {
        get => new DateTimeOffset(StartUtc, TimeSpan.Zero);
        set => StartUtc = value.UtcDateTime;
    }

    /// <summary>
    /// End time exposed as DateTimeOffset? for your app logic/UI.
    /// Not mapped; backing storage is EndUtc.
    /// </summary>
    [NotMapped]
    public DateTimeOffset? EndTime
    {
        get => (EndUtc == null) ? null : new DateTimeOffset(EndUtc.Value, TimeSpan.Zero);
        set => EndUtc = value?.UtcDateTime;
    }

    /// <summary>
    /// Optional note for the shift.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Navigation property to the associated project.
    /// </summary>
    public Project Project { get; set; } = null!; // Navigation property

    /// <summary>
    /// Gets the duration of the shift.
    /// Returns zero if the shift is open.
    /// Computed from persisted UTC fields.
    /// </summary>
    [NotMapped]
    public TimeSpan Duration
    {
        get => EndUtc.HasValue ? EndUtc.Value - StartUtc : TimeSpan.Zero;
    }

    /// <summary>
    /// True if shift has no end time.
    /// </summary>
    public bool IsOpen 
    {
        get => EndUtc == null;
    }
}