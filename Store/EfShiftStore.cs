using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Store;


/// <summary>
/// Adapter from your EF Core repos/services to the UI command needs.
/// Uses GetAwaiter().GetResult() because Terminal.Gui callbacks are sync.
/// </summary>
internal sealed class EfShiftStore : IShiftStore
{

    private readonly IProjectRepository _projects;
    private readonly IShiftRepository _shifts;
    private readonly IShiftService _service;

    public EfShiftStore(IProjectRepository projects, IShiftRepository shifts, IShiftService service)
    {
        _projects = projects;
        _shifts = shifts;
        _service = service;
    }

    public Project EnsureProject(string nameOrId)
    {
        if (int.TryParse(nameOrId, out int id))
        {
            Project? byId = _projects.GetByIdAsync(id).GetAwaiter().GetResult();
            return byId ?? throw new InvalidOperationException("Project not found.");
        }

        // "Ensure" semantics: create if missing, otherwise return existing
        Project? existing = _projects.GetByNameAsync(nameOrId).GetAwaiter().GetResult();
        if (existing is not null)
            return existing;
        

        Project created = _projects.CreateAsync(nameOrId).GetAwaiter().GetResult();
        return created;
    }

    #region Getters
    public List<Project> GetAllProjects()
    {
        return _projects.GetAllAsync().GetAwaiter().GetResult();
    }

    public Shift? GetActiveShift()
    {
        List<Shift> all = _shifts.GetAllAsync().GetAwaiter().GetResult();

        for (int index = 0; index < all.Count; index++)
        {
            if (all[index].IsOpen)
            {
                return all[index];
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

    #endregion


    #region Setters
    public Shift ClockIn(string projectNameOrId, string? note)
    {
        return _service.StartShiftAsync(projectNameOrId, note).GetAwaiter().GetResult();
    }

    public void ClockOut(long id)
    {
        _service.EndShiftAsync(id).GetAwaiter().GetResult();
    }
    #endregion


    public Dictionary<string, TimeSpan> TotalsByProject()
    {
        return _shifts.TotalsByProjectAsync().GetAwaiter().GetResult();
    }

    public void DeleteShift(long id)
    {
        _shifts.DeleteAsync(id).GetAwaiter().GetResult();
    }

}
