namespace BlazorCookbookApp.Client.Shared;

/// <summary>
/// Represents an action that occurred during the render mode lifecycle.
/// Used for tracking the progression from initial static rendering to interactive state.
/// </summary>
public class RenderAction
{
    /// <summary>
    /// Timestamp when the action occurred (HH:mm:ss.fff format).
    /// </summary>
    public string Time { get; set; } = string.Empty;
    
    /// <summary>
    /// Duration since component start in milliseconds.
    /// </summary>
    public double DurationMs { get; set; }
    
    /// <summary>
    /// Description of what happened during this action.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Category of the action for grouping purposes.
    /// </summary>
    public RenderActionCategory Category { get; set; }
    
    /// <summary>
    /// Render mode that was active during this action.
    /// </summary>
    public string? RenderMode { get; set; }
}

/// <summary>
/// Categories for grouping render actions by lifecycle phase.
/// </summary>
public enum RenderActionCategory
{
    /// <summary>
    /// Component initialization and setup actions.
    /// </summary>
    Initialization,
    
    /// <summary>
    /// Transitioning from static to interactive mode.
    /// </summary>
    Transition,
    
    /// <summary>
    /// Active interactive state actions.
    /// </summary>
    Active,
    
    /// <summary>
    /// User interaction events.
    /// </summary>
    Interaction,
    
    /// <summary>
    /// Server-side processing phase (Auto mode).
    /// </summary>
    ServerPhase,
    
    /// <summary>
    /// Transitioning from server to client (Auto mode).
    /// </summary>
    ClientTransition,
    
    /// <summary>
    /// Client-side processing phase (Auto mode).
    /// </summary>
    ClientActive,
    
    /// <summary>
    /// WebAssembly runtime hydration phase.
    /// </summary>
    ClientHydration,
    
    /// <summary>
    /// User interaction events.
    /// </summary>
    UserInteraction
} 