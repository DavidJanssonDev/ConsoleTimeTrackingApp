using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTimeTrackingApp.Models
{
    /// <summary>
    /// Represents a project that shifts can be logged against.
    /// A project acts as the parent entity in a one-to-many relationship:
    /// one Project can have many Shifts.
    /// </summary>
    internal sealed class Project
    {
        /// <summary>
        /// Primary key for the Project entity.
        /// EF Core maps this to the Projects.Id column.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Human-readable unique project name (e.g. "Client A - Website").
        /// This is enforced to be unique in <see cref="Data.Database.OnModelCreating"/>.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Navigation property representing all shifts that belong to this project.
        /// EF Core uses this to create the one-to-many relationship.
        /// </summary>
        public List<Shift> Shifts { get; set; } = [];
    }
}
