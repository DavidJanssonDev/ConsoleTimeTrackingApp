using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTimeTrackingApp.Models
{
    /// <summary>
    /// Represents a work shift with start and end times, and an optional note.
    /// </summary>
    internal class Shift
    {
        public long Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public string? Note { get; set; }
    }
}
