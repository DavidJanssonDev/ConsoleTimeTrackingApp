using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ConsoleTimeTrackingApp.Models;

namespace ConsoleTimeTrackingApp.Data
{
    /// <summary>
    /// Database helper: creates DB file + tables if they don't exist
    /// </summary>
    internal class Database : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=timetracking.db");
        }
   

    }
}
