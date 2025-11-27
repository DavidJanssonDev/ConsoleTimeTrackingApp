using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleTimeTrackingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTimeTrackingApp.Data
{
    /// <summary>
    /// Repository responsible for Project CRUD and lookup logic.
    /// Keeps EF Core access out of UI/menu code.
    /// </summary>
    internal sealed class ProjectRepository
    {
        private readonly Database _db;

        /// <summary>
        /// Creates a repostory using the given DbContext.
        /// The context lifetime is manged by the caller.
        /// </summary>
        public ProjectRepository(Database db)
        {
            _db = db;
        }


        /// <summary>
        /// Creates a new project if it doesn't exist, otherwise returns the existing one.
        /// </summary>
        /// <param name="name">Project name typed by user.</param>
        /// <returns>The newly created project or the existing match.</returns>
        public async Task<Project>CreateAsync(string name)
        {

            // Normalize input to avoid duplicates form extra spaces.
            string normalized = name.Trim();

            // If User typed nothing, fail early rather then storing garbage.
            if (string.IsNullOrEmpty(normalized))
                throw new System.ArgumentException("Project name cannot be empty.");

            // try find existing proejct with same normalized name.
            Project? existing = await _db.Projects
                .FirstOrDefaultAsync(project => project.Name == normalized);

            if (existing != null)
                return existing;

            // Create and persist a new project.
            Project project = new Project { Name = normalized };

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return project;
        }

        ///<summary>
        /// Returns all project sorted by name
        /// </summary>
        public Task<List<Project>> GetAllAsync()
        {
            return _db.Projects
                .OrderBy(project => project.Name)
                .ToListAsync();
        }

        ///<summary>
        /// Finds a proejct by its primary key
        /// </summary>
        public Task<Project?> GetByIdAsync(int id)
        {
            return _db.Projects
                .FirstOrDefaultAsync(project => project.Id == id);
        }

        /// <summary>
        /// Finds a project by exact name match after trimming whitespace
        /// </summary>
        public Task<Project?> GetByNameAsync(string name)
        {
            string normalized = name.Trim();

            return _db.Projects
                .FirstOrDefaultAsync(project => project.Name == normalized);

        }

    }
}
