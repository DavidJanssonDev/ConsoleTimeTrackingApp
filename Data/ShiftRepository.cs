using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTimeTrackingApp.Models;
using ConsoleTimeTrackingApp.Data;
using Microsoft.Data.Sqlite;

namespace ConsoleTimeTrackingApp.Data
{
    /// <summary>
    /// Repository: ALL SQL for shifts lives here.
    /// UI/Menu code should call these methods (usually via a Service).
    /// </summary>
    internal class ShiftRepository
    {
        private readonly Database _db;

        public ShiftRepository(Database db)
        {
            _db = db;
        }

    }
}
