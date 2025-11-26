using ConsoleTimeTrackingApp.Data;
using ConsoleTimeTrackingApp.Models;

using var db = new Database();

await db.Database.EnsureCreatedAsync();

// Note: This sample requires the database to be created before running.

// Create
Console.WriteLine("Inserting a new blog");
db.Add( new Shift { EndTime = DateTimeOffset.UtcNow, StartTime = DateTimeOffset.Now});
await db.SaveChangesAsync();