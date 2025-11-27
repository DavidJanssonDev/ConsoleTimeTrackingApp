using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTimeTrackingApp.Models;
using ConsoleTimeTrackingApp.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTimeTrackingApp.Data
{
    /// <summary>
    /// Repository responsible for Shift operations and summary queries.
    /// </summary>
    internal class ShiftRepository
    {
        private readonly Database _db;

        /// <summary>
        /// Creates a repository using the given DbContext.
        /// </summary>
        public ShiftRepository(Database db)
        {
            _db = db;
        }

        /// <summary>
        /// Inserts a new shift into the database.
        /// </summary>
        /// <param name="shift">Shift to insert.</param>
        /// <returns>The inserted shift (with generated Id).</returns>
        public async Task<Shift> AddAsync(Shift shift)
        {
            _db.Shifts.Add(shift);
            await _db.SaveChangesAsync();
            return shift;
        }


        /// <summary>
        /// Retruns all shfits, newest first, including their Project navigation.
        /// </summary>
        public Task<List<Shift>> GetAllAsync()
        {
            return _db.Shifts
                .Include(s => s.Project)               // we want Project.Name available
                .OrderByDescending(s => s.StartTime)   // newest on top
                .ToListAsync();
        }

        /// <summary>
        /// Returns a shift by Id, including its Project.
        /// </summary>
        public Task<Shift?> GetByIdAsync(long id)
        {
            return _db.Shifts
                .Include(s => s.Project)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Closes (ends) a shift by setting EndTime.
        /// </summary>
        /// <param name="id">Shift Id.</param>
        /// <param name="endTime">Time to set as EndTime.</param>
        public async Task EndAsync(long id, DateTimeOffset endTime)
        {
            Shift? shift = await GetByIdAsync(id);
            
            if (shift is null)
                throw new InvalidOperationException($"Shift {id} not found.");

            // Validate that the shift isn't already ended, optional but nice.
            if (shift.EndTime != null)
                throw new InvalidOperationException($"Shift {id} is already ended.");

            // Validate chronological order.
            if (endTime <= shift.StartTime)
                throw new InvalidOperationException($"End time must be after start time.");

            shift.EndTime = endTime;
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a shift if it exists.
        /// </summary>
        public async Task DeleteAsync(long id)
        {
            var shift = await GetByIdAsync(id);
            if (shift == null)
                return; // no-op if Id doesn't exist

            _db.Shifts.Remove(shift);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Computes total time per project using only closed shifts.
        /// This version runs the grouping and summing in the database,
        /// so performance stays good even with many shifts.
        /// </summary>
        /// <returns>
        /// Dictionary where key = project name, value = total duration.
        /// </returns>
        public async Task<Dictionary<string, TimeSpan>> TotalsByProjectAsync()
        {
            // Build Querty with explicit type.
            IQueryable<ProjectTotalRow> query =
                _db.Shifts
                    .Where(shift => shift.EndTime != null)
                    .GroupBy(shift => shift.Project.Name)
                    .Select(group => new ProjectTotalRow
                    {
                        ProjectName = group.Key,

                        // Sum duration in ticks inside SQL.
                        // "!" is safe bevuase we filtered EndTime != null above.
                        TotalTicks = group.Sum(shift => (shift.EndTime!.Value - shift.StartTime).Ticks)
                    });

            // Materialize results explicitly typed
            List<ProjectTotalRow> rows = await query.ToListAsync();

            Dictionary<string, TimeSpan> totals =
                rows.ToDictionary(
                        row => row.ProjectName,
                        row => TimeSpan.FromTicks(row.TotalTicks)
                    );

            return totals;
        }
    }
}
