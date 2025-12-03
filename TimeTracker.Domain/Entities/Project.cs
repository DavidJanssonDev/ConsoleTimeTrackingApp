namespace TimeTracker.Domain.Entities;

/// <summary>
/// Represents a project that contains a collection of work shifts.
/// </summary>
public class Project
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of shifts associated with the project.
    /// </summary>
    public ICollection<Shift> Shifts { get; set; } = [];
}