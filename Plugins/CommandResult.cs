using TimeTracker.MenuModel.Interfaces;

namespace TimeTracker.Plugins;

public abstract record CommandResult;

public sealed record StayResult : CommandResult;

public sealed record BackResult : CommandResult;

public sealed record NavigateToResult(IMenuElement Target) : CommandResult;

public sealed record ReplaceCurrentResult(IMenuElement Target) : CommandResult;

public sealed record ShowMessageResult(string Title, string Message) : CommandResult;
