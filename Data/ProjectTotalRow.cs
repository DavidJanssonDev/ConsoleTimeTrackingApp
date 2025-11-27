using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTimeTrackingApp.Data
{
    /// <summary>
    /// Raw aggregation row returned from the database query.
    /// We aggregate in ticks because SQLite doesn't have a TimeSpan type.
    /// </summary>
    internal sealed class ProjectTotalRow
    {
        ///<summary>
        /// Name of the project (group key)
        /// </summary>
        public string ProjectName { get; set; } = "";

        ///<summary>
        /// Summed duration in tickes for all closed shifts in this project
        /// </summary>
        public long TotalTicks { get; set; }
    }
}
