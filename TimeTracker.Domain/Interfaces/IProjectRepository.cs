namespace TimeTracker.Domain.Interfaces;

/// <summary>
/// Provides methods for creating, retrieving, and listing <see cref="Project"/> entities.
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Creates a new <see cref="Project"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the project to create.</param>
    /// <returns>The created <see cref="Project"/>.</returns>
    Task<Project> CreateAsync(string name);

    /// <summary>
    /// Retrieves a <see cref="Project"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <returns>The <see cref="Project"/> if found; otherwise, <c>null</c>.</returns>
    Task<Project?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a <see cref="Project"/> by its name.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    /// <returns>The <see cref="Project"/> if found; otherwise, <c>null</c>.</returns>
    Task<Project?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves all <see cref="Project"/> entities.
    /// </summary>
    /// <returns>A list of all <see cref="Project"/> entities.</returns>
    Task<List<Project>> GetAllAsync();
}
