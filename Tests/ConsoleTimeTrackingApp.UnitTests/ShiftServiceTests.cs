using TimeTracker.Application.Services;
using Xunit;

namespace TimeTracker.Tests.ShiftServiceTests;
/// <summary>
/// Contains unit tests for the <see cref="ShiftService"/> class.
/// </summary>
public class ShiftServiceTests
{
    private class InMemoryProjectRepo : IProjectRepository
    {
        public List<Project> Projects = [];
        public Task<Project> CreateAsync(string name)
        {
            string norm = name.Trim();
            var existing = Projects.Find(p => p.Name == norm);
            if (existing != null) return Task.FromResult(existing);
            var project = new Project { Id = Projects.Count + 1, Name = norm };
            Projects.Add(project);
            return Task.FromResult(project);
        }
        public Task<Project?> GetByIdAsync(int id) =>
            Task.FromResult<Project?>(Projects.Find(p => p.Id == id));
        public Task<Project?> GetByNameAsync(string name) =>
            Task.FromResult<Project?>(Projects.Find(p => p.Name == name.Trim()));
        public Task<List<Project>> GetAllAsync() => Task.FromResult(new List<Project>(Projects));
    }

    private class InMemoryShiftRepo : IShiftRepository
    {
        public List<Shift> Shifts = new();
        public Task<Shift> AddAsync(Shift shift)
        {
            shift.Id = Shifts.Count + 1;
            // link project
            shift.Project = shift.Project ?? new Project { Id = shift.ProjectId };
            Shifts.Add(shift);
            return Task.FromResult(shift);
        }
        public Task<List<Shift>> GetAllAsync() => Task.FromResult(new List<Shift>(Shifts));
        public Task<Shift?> GetByIdAsync(long id) => Task.FromResult<Shift?>(Shifts.Find(s => s.Id == id));
        public Task EndAsync(long id, DateTimeOffset endTime)
        {
            var shift = Shifts.Find(s => s.Id == id);
            if (shift == null) throw new InvalidOperationException("Shift not found");
            if (shift.EndTime != null) throw new InvalidOperationException("Already ended");
            if (endTime <= shift.StartTime) throw new InvalidOperationException("End must be after start");
            shift.EndTime = endTime;
            return Task.CompletedTask;
        }
        public Task DeleteAsync(long id)
        {
            Shifts.RemoveAll(s => s.Id == id);
            return Task.CompletedTask;
        }
        public Task<Dictionary<string, TimeSpan>> TotalsByProjectAsync()
        {
            // Not needed for this test example
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Tests that starting a shift with a new project name creates both the project and the shift.
    /// </summary>
    [Fact]
    public async Task StartShiftAsyncWithNewProjectCreatesProjectAndShift()
    {

        // Arrange

        InMemoryProjectRepo projRepo = new();

        InMemoryShiftRepo shiftRepo = new();

        ShiftService service = new(projRepo, shiftRepo);

        string newProjectName = "TestProject";

        string note = "Test note";



        // Act

        Shift resultShift = await service.StartShiftAsync(newProjectName, note);

        // Assert
        // The new project should be created and stored

        Assert.NotNull(resultShift);
        Assert.Equal(newProjectName, resultShift.Project.Name);
        Assert.Contains(projRepo.Projects, p => p.Name == newProjectName);

        // Shift should be in repository with an assigned Id and no EndTime

        Assert.NotEqual(0, resultShift.Id);
        Assert.Null(resultShift.EndTime);
        Assert.Contains(shiftRepo.Shifts, s => s.Id == resultShift.Id);

    }
}
