using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTimeTrackingApp.Models
{
    /// <summary>
    /// Represents a single work shift.
    /// A shift belongs to exactly one project (many shifts -> one project).
    /// </summary>
    internal sealed class Shift
    {
        /// <summary>
        /// Primary key for the Shift entity.
        /// EF Core maps this to Shifts.Id.
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// Foreign key pointing to the owning project.
        /// EF Core maps this to Shifts.ProjectId.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Navigation property to the owning project.
        /// EF fills this when shifts are loaded from the database
        /// (usually via Include()).
        /// </summary>
        public Project Project { get; set; } = null!;


        /// <summary>
        /// When the shift started.
        /// Stored in the DB as a DateTimeOffset.
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// When the shift ended. If null, the shift is still open.
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Optional free-text note for extra context.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Calculated duration of the shift.
        /// If the shift is open (EndTime null), duration is zero
        /// so it won't accidentally count toward totals.
        /// </summary>
        public TimeSpan Duration =>
            EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;

    }
}
