using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Store;

/// <summary>
/// Minimal surface the UI/commands need.
/// Public so plugins can use it.
/// </summary>
public interface IShiftStore
{
    Project EnsureProject(string nameOrId);
    List<Project> GetAllProjects();

    Shift StartShift(string projectNameOrId, string? note);
    void EndShift(long id);

    Shift? GetActiveShift();
    List<Shift> GetShiftsForDate(DateTime localDate);
    List<Shift> GetShiftsForDateRange(DateTime startLocal, DateTime endLocal);

    Dictionary<string, TimeSpan> TotalsByProject();
    void DeleteShift(long id);
}

