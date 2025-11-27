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
        public DbSet<Project> Projects { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        private const string ConnectionString = "Data Source=timetracking.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------------------------
            // Configure Project entity
            // ---------------------------
            modelBuilder.Entity<Project>(entity =>
            {
                // Configure the Project.Name property (DB column: Projects.Name).
                entity.Property(p => p.Name)
                    // IsRequired() => database column becomes NOT NULL.
                    // This ensures every project has a usable name.
                    .IsRequired()
                    // HasMaxLength(200) => EF validates length before saving.
                    // In many providers it also sets the column size.
                    // Prevents accidental huge strings in the DB.
                    .HasMaxLength(200);

                // Create an index on Project.Name.
                // Indexes speed up searching/filtering by this column.
                // Example speedup: FirstOrDefault(p => p.Name == "Alpha")
                entity.HasIndex(p => p.Name)
                    // IsUnique() => database guarantees no duplicate project names.
                    // Without this, totals could split between "Alpha" and another "Alpha".
                    // If a duplicate insert happens, SaveChanges throws a DbUpdateException.
                    .IsUnique();
            });

            // ---------------------------
            // Configure Shift entity
            // ---------------------------
            modelBuilder.Entity<Shift>(entity =>
            {
                // Configure optional Note column length.
                entity.Property(s => s.Note)
                    // The note is optional (nullable), so no IsRequired().
                    // Max length keeps DB tidy and prevents abuse.
                    .HasMaxLength(500);

                // Relationship definition:
                // Shift -> Project is many-to-one
                // Project -> Shifts is one-to-many.
                entity.HasOne(s => s.Project)          // Each Shift has ONE Project
                    .WithMany(p => p.Shifts)          // Each Project has MANY Shifts
                    .HasForeignKey(s => s.ProjectId)  // ProjectId is the FK column in Shifts table
                                                      // Cascade delete means:
                                                      // If a Project is deleted, all related Shifts are deleted automatically.
                                                      // This prevents "orphaned" shifts pointing to non-existent projects.
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(shift => shift.StartTime)
                    .HasConversion(
                        value => value.ToUnixTimeMilliseconds(),
                        value => DateTimeOffset.FromUnixTimeMilliseconds(value)
                    )
                    .HasColumnType("INTEGER");

                entity.Property(shift => shift.EndTime)
                    .HasConversion(
                        value => value.HasValue ? value.Value.ToUnixTimeMilliseconds() : (long?)null,
                        value => value.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(value.Value) : (DateTimeOffset?)null
                    )
                    .HasColumnType("INTEGER");


            });

            // Call base implementation (good practice even if it currently does nothing).
            base.OnModelCreating(modelBuilder);
        }
    }
}
