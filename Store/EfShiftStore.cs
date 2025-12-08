using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Store;


/// <summary>
/// Adapter from your EF Core repos/services to the UI command needs.
/// Uses GetAwaiter().GetResult() because Terminal.Gui callbacks are sync.
/// </summary>
internal sealed class EfShiftStore(IProjectRepository projects, IShiftRepository shifts, IShiftService service) : IShiftStore
{

    private readonly IProjectRepository _projects = projects;
    private readonly IShiftRepository _shifts = shifts;
    private readonly IShiftService _service = service;

    public Project EnsureProject(string nameOrId)
    {
        if (int.TryParse(nameOrId, out int id))
        {
            Project? byId = _projects.GetByIdAsync(id).GetAwaiter().GetResult();
            return byId ?? throw new Exception("Project not found.");
        }

        Project created = _projects.CreateAsync(nameOrId).GetAwaiter().GetResult();
        return created;
    }

    public List<Project> GetAllProjects()
    {
        return _projects.GetAllAsync().GetAwaiter().GetResult();
    }

    public Shift StartShift(string projectNameOrId, string? note)
    {
        return _service.StartShiftAsync(projectNameOrId, note).GetAwaiter().GetResult();
    }

    public void EndShift(long id)
    {
        _service.EndShiftAsync(id).GetAwaiter().GetResult();
    }

    public Shift? GetActiveShift()
    {
        List<Shift> all = _shifts.GetAllAsync().GetAwaiter().GetResult();
        for (int i = 0; i < all.Count; i++)
        {
            if (all[i].IsOpen)
            {
                return all[i];
            }
        }
        return null;
    }

    public List<Shift> GetShiftsForDate(DateTime localDate)
    {
        DateTime start = localDate.Date;
        DateTime end = start.AddDays(1);
        return GetShiftsForDateRange(start, end);
    }

    public List<Shift> GetShiftsForDateRange(DateTime startLocal, DateTime endLocal)
    {
        List<Shift> all = _shifts.GetAllAsync().GetAwaiter().GetResult();
        List<Shift> list = [];

        for (int i = 0; i < all.Count; i++)
        {
            Shift s = all[i];
            DateTime localStart = s.StartTime.ToLocalTime().DateTime;

            if (localStart >= startLocal && localStart < endLocal)
            {
                list.Add(s);
            }
        }

        return list;
    }

    public Dictionary<string, TimeSpan> TotalsByProject()
    {
        return _shifts.TotalsByProjectAsync().GetAwaiter().GetResult();
    }

    public void DeleteShift(long id)
    {
        _shifts.DeleteAsync(id).GetAwaiter().GetResult();
    }

}
