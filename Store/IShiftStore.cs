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

    Shift ClockIn(string projectNameOrId, string? note);
    void ClockOut(long id);

    Shift? GetActiveShift();
    Shift? GetShiftForDate(DateTime today);
    List<Shift> GetShiftsForDate(DateTime localDate);
    List<Shift> GetShiftsForDateRange(DateTime startLocal, DateTime endLocal);

    Dictionary<string, TimeSpan> TotalsByProject();
    void DeleteShift(long id);
}

