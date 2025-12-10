# ConsoleTimeTrackingApp

A lightweight terminal-based time tracker for clocking in/out on projects, viewing reports, and managing entries — built with **Terminal.Gui**, **EF Core**, and **SQLite**. The UI is menu-driven and supports a simple plugin system for adding your own commands.

The app persists all shifts and projects locally in a SQLite database file named `timetracker.db`.

---

## Features

- **Clock in / Clock out**
  - Start a shift on an existing project or create a new one on the fly.
- **Project management**
  - Create projects and view project list via a dynamic submenu.
- **Entry management**
  - Browse recent shifts and delete entries.
- **Reports**
  - View today’s shifts, current week, and totals per project.
- **Plugin support**
  - Drop-in commands are loaded from a local `plugins/` folder at startup.
- **Terminal UI**
  - Simple keyboard navigation and status/footer updates.

---

## Tech Stack

- **.NET 10 (Console Exe)**
- **Terminal.Gui** for interactive terminal UI
- **Entity Framework Core + SQLite** for persistence

---

## How It Works (High Level)

1. **App bootstraps Terminal.Gui** and creates UI controls.
2. **SQLite DbContext is created** and schema ensured.
3. **Repositories + ShiftService** are wired up.
4. An **`IShiftStore` adapter** (`EfShiftStore`) bridges UI commands to EF persistence.
5. **Built-in commands register**, then plugins are loaded.
6. A **menu tree is built from commands**, rendered in the UI, and navigated via a stack.

---

## Getting Started

### Prerequisites
- .NET SDK 10.x installed

### Run the app

```bash
# from the repo root

# restore NuGet deps
dotnet restore

# (optional but nice) build to catch errors early
dotnet build

# run the app
dotnet run

# --- optional extras ---

# run in Release mode
dotnet run -c Release

# publish a self-contained release (example: macOS arm64)
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true

# publish framework-dependent (smaller output)
dotnet publish -c Release

```
On first run, a timetracker.db SQLite file will be created beside the executable.

---

### Controls

- **↑ / ↓** : move selection  
- **Enter** : open menu / execute command  
- **Esc / Backspace** : go back  

These are also shown in the UI header.

---
## Project Structure

```text
TimeTracker/
├── ApplicationCode/
│   └── Services/            # ShiftService, domain orchestration
├── Commands/
│   ├── Timeshift/           # Clock In/Out
│   ├── Reports/             # View Today/Week, Totals
│   ├── Management/          # Projects, Entries
│   └── System/              # Quit
├── Domain/
│   ├── Entities/            # Project, Shift
│   └── Interfaces/          # Repository/service interfaces
├── Infrastructre/
│   └── Persistence/         # EF Core DbContext + repositories
├── MenuModel/               # MenuNode, MenuItem, builders
├── Plugins/                 # PluginLoader + plugin interfaces
├── Store/                   # IShiftStore + EfShiftStore adapter
├── UI/                      # UiFactory, UiState, navigation helpers
└── Program.cs               # Composition root / startup
```
---
## Adding a Plugin Command

Plugins are loaded from the `plugins/` folder at startup.

General idea:

1. Create a class library project that references the main project.
2. Implement a concrete class that implements `ICommand`.
3. Build and drop the DLL into `plugins/`.

The loader will:

- scan the folder for DLLs  
- find non-abstract `ICommand` implementations  
- instantiate them (prefers a constructor that takes `IShiftStore`)  
- register them into the command registry  

Example (pseudo):

```csharp

public sealed class MyCommand : ICommand
{
    public string DisplayName => "My Custom Action";
    public string Category => "Custom";
    public bool CanHaveSubmenu => false;
    public IShiftStore ShiftStore { get; set; }

    public MyCommand(IShiftStore store)
    {
        ShiftStore = store;
    }

    public void Execute()
    {
        // your logic here
    }
}
```
Once the DLL is in plugins/, it should appear automatically in the root menu under its category.

---

## Data Model

- **Project**
  - `Id`, `Name`

- **Shift**
  - `Id`, `ProjectId`, `Project`, `StartTime`, `EndTime?`, `Note?`, `IsOpen`

Shifts are started/stopped through `ShiftService` and persisted via repositories.

---

## Roadmap Ideas

- Export to CSV / JSON
- Edit shift notes
- Custom date-range reporting
- Multi-active shift prevention

---

## License
This project is licensed under the MIT License — see the `LICENSE` file for details.