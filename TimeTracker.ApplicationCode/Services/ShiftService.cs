namespace TimeTracker.ApplicationCode.Services;

/// <summary>
/// Provides services for managing work shifts, including starting and ending shifts for projects.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ShiftService"/> class.
/// </remarks>
/// <param name="projectRepo">The project repository.</param>
/// <param name="shiftRepo">The shift repository.</param>
public class ShiftService(IProjectRepository projectRepo, IShiftRepository shiftRepo) : IShiftService
{
    private readonly IProjectRepository _projectRepo = projectRepo;
    private readonly IShiftRepository _shiftRepo = shiftRepo;

    /// <summary>
    /// Starts a new shift for the specified project or project name.
    /// </summary>
    /// <param name="projectNameOrId">The project name or ID.</param>
    /// <param name="note">An optional note for the shift.</param>
    /// <returns>The started <see cref="Shift"/>.</returns>
    public async Task<Shift> StartShiftAsync(string projectNameOrId, string? note)
    {
        Project project;

        if (int.TryParse(projectNameOrId, out var id))
            project = await _projectRepo.GetByIdAsync(id) ?? throw new Exception("Project not found");
        else
            project = await _projectRepo.CreateAsync(projectNameOrId);

        Shift shift = new ()
        {
            ProjectId = project.Id,
            StartTime = DateTimeOffset.Now,
            Note = string.IsNullOrWhiteSpace(note) ? null : note,
            Project = project
        };

        return await _shiftRepo.AddAsync(shift);
    }

    /// <summary>
    /// Ends the specified shift.
    /// </summary>
    /// <param name="shiftId">The ID of the shift to end.</param>
    public async Task EndShiftAsync(long shiftId) => await _shiftRepo.EndAsync(shiftId, DateTimeOffset.Now);
    
}

