using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorCookbookApp.Client.Shared;

/// <summary>
/// Base class for render mode demonstration components, providing common functionality
/// for educational delays, render mode detection, status display, and journey tracking.
/// </summary>
public abstract class RenderModeComponentBase : ComponentBase
{
    #region Common Fields
    
    /// <summary>
    /// Indicates if the educational delay is currently active to show static phase
    /// </summary>
    protected bool _isDelayed = true;
    
    /// <summary>
    /// Component start time for duration calculations
    /// </summary>
    protected DateTime _startTime = DateTime.UtcNow;
    
    /// <summary>
    /// Time when component became interactive (if applicable)
    /// </summary>
    protected DateTime? _interactiveTime = null;
    
    /// <summary>
    /// Educational delay duration in milliseconds to make static phase visible
    /// </summary>
    protected const int STATIC_PHASE_DELAY_MS = 1500;
    
    /// <summary>
    /// Action history for tracking component lifecycle events (optional - used by Server/Auto pages)
    /// </summary>
    protected List<RenderAction> _actionHistory = new();
    
    /// <summary>
    /// Journey tracking for all render mode changes
    /// </summary>
    protected List<RenderModeState> _renderModeJourney = new();
    
    /// <summary>
    /// Current render mode for tracking changes
    /// </summary>
    protected string? _currentRenderMode = null;
    
    #endregion

    #region Abstract Properties
    
    /// <summary>
    /// Page title specific to the render mode being demonstrated
    /// </summary>
    protected abstract string PageTitle { get; }
    
    /// <summary>
    /// Summary description of the render mode behavior
    /// </summary>
    protected abstract string PageSummary { get; }
    
    #endregion

    #region Common Display Methods
    
    /// <summary>
    /// Gets the display render mode, showing "Static" during educational delay
    /// </summary>
    /// <returns>Current render mode for display purposes</returns>
    protected string GetDisplayRenderMode()
    {
        // During educational delay, show "Static" to simulate actual static phase
        return _isDelayed ? "Static" : (GetCurrentRenderMode() ?? "Unknown");
    }

    /// <summary>
    /// Gets the interactive status, showing false during educational delay
    /// </summary>
    /// <returns>Whether component appears interactive to user</returns>
    protected bool GetDisplayInteractive()
    {
        // During educational delay, show false to simulate actual static phase
        return !_isDelayed && GetCurrentInteractive();
    }

    /// <summary>
    /// Gets the CSS class for render mode badge based on current mode
    /// </summary>
    /// <returns>Bootstrap CSS classes for render mode styling</returns>
    protected string GetRenderModeClass()
    {
        var mode = GetDisplayRenderMode();
        return GetRenderModeClass(mode);
    }

    /// <summary>
    /// Gets CSS class for render mode badge with specific mode override
    /// </summary>
    /// <param name="renderMode">Specific render mode to get class for</param>
    /// <returns>Bootstrap CSS classes for the specified render mode</returns>
    protected string GetRenderModeClass(string? renderMode)
    {
        return renderMode?.ToLower() switch
        {
            "webassembly" => "bg-success text-white",    // Green for WebAssembly
            "server" => "bg-primary text-white",         // Blue for Server
            "static" => "bg-warning text-dark",          // Yellow for Static
            _ => "bg-secondary text-white"               // Gray for unknown
        };
    }

    #endregion

    #region Journey Tracking (Universal)

    /// <summary>
    /// Gets the render mode journey for display
    /// </summary>
    /// <returns>List of render mode states in chronological order</returns>
    protected List<RenderModeState> GetRenderModeJourney()
    {
        return _renderModeJourney;
    }

    /// <summary>
    /// Determines if the journey section should be shown
    /// </summary>
    /// <returns>True if there are multiple modes or transitions to show</returns>
    protected bool ShouldShowJourney()
    {
        return _renderModeJourney.Count > 1;
    }

    /// <summary>
    /// Adds a render mode to the journey tracking
    /// </summary>
    /// <param name="mode">Render mode name</param>
    /// <param name="duration">Duration since start</param>
    protected void AddRenderModeToJourney(string mode, string duration)
    {
        // Only add if it's different from the last mode to avoid duplicates
        if (!_renderModeJourney.Any() || _renderModeJourney.Last().Mode != mode)
        {
            var state = new RenderModeState { Mode = mode, Duration = duration };
            _renderModeJourney.Add(state);
            OnJourneyUpdated(state);
        }
    }

    /// <summary>
    /// Automatically detects render mode changes and updates journey
    /// </summary>
    protected void DetectAndTrackRenderModeChanges()
    {
        var currentMode = GetDisplayRenderMode();
        
        if (_currentRenderMode != currentMode)
        {
            var oldMode = _currentRenderMode;
            _currentRenderMode = currentMode;
            
            // Calculate duration since start
            var duration = $"{GetDurationSinceStart():F0}ms";
            
            // Add to journey
            AddRenderModeToJourney(currentMode, duration);
            
            // Notify derived classes
            OnRenderModeChanged(oldMode, currentMode);
        }
    }

    #endregion

    #region Action History Support (Optional - for Server/Auto pages)

    /// <summary>
    /// Adds an action to the history for tracking component lifecycle events
    /// </summary>
    /// <param name="description">Description of the action</param>
    /// <param name="category">Category of the action</param>
    protected void AddAction(string description, RenderActionCategory category)
    {
        var now = DateTime.UtcNow;
        string? renderMode = null;
        
        try
        {
            renderMode = GetCurrentRenderMode();
        }
        catch (InvalidOperationException)
        {
            // RendererInfo is not available in unit tests - this is expected
            renderMode = "Test";
        }
        
        _actionHistory.Add(new RenderAction
        {
            Time = now.ToString("HH:mm:ss.fff"),
            DurationMs = (now - _startTime).TotalMilliseconds,
            Description = description,
            Category = category,
            RenderMode = renderMode
        });
    }

    /// <summary>
    /// Gets actions grouped by category for display purposes
    /// </summary>
    /// <returns>Dictionary of actions grouped by category</returns>
    protected Dictionary<string, List<RenderAction>> GetActionsByCategory()
    {
        return _actionHistory
            .GroupBy(a => a.Category.ToString())
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// Gets whether this component uses action history tracking
    /// </summary>
    /// <returns>True if action history is being used</returns>
    protected bool HasActionHistory => _actionHistory.Any();

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Initialize component with common setup
    /// </summary>
    protected override void OnInitialized()
    {
        _startTime = DateTime.UtcNow;
        
        // Initialize journey with starting mode
        var initialMode = GetDisplayRenderMode();
        _currentRenderMode = initialMode;
        AddRenderModeToJourney(initialMode, "0ms");
        
        // Call virtual method for page-specific initialization
        OnInitializedOverride();
        
        base.OnInitialized();
    }

    /// <summary>
    /// Handle educational delay, render mode detection, and journey tracking
    /// </summary>
    /// <param name="firstRender">Whether this is the first render</param>
    /// <returns>Async task</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Handle educational delay to make static phase visible
            if (_isDelayed)
            {
                await HandleEducationalDelayAsync();
                _isDelayed = false;
                await OnEducationalDelayCompleted();
                
                // Check for mode changes after delay
                DetectAndTrackRenderModeChanges();
                StateHasChanged();
            }
            
            // Track interactive time
            if (GetCurrentInteractive() && !_interactiveTime.HasValue)
            {
                _interactiveTime = DateTime.UtcNow;
            }
            
            // Call virtual method for page-specific first render logic
            await OnAfterFirstRenderAsync();
        }
        else
        {
            // Always check for render mode changes on subsequent renders
            DetectAndTrackRenderModeChanges();
            
            // Call virtual method for page-specific subsequent render logic
            await OnAfterSubsequentRenderAsync();
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    #region Educational Delay Handling

    /// <summary>
    /// Handles the educational delay to show static rendering phase
    /// </summary>
    /// <returns>Async task</returns>
    protected virtual async Task HandleEducationalDelayAsync()
    {
        await Task.Delay(STATIC_PHASE_DELAY_MS);
    }

    /// <summary>
    /// Called after educational delay completes - override for custom behavior
    /// </summary>
    /// <returns>Async task</returns>
    protected virtual Task OnEducationalDelayCompleted()
    {
        return Task.CompletedTask;
    }

    #endregion

    #region Virtual Methods for Override

    /// <summary>
    /// Override for page-specific initialization logic
    /// </summary>
    protected virtual void OnInitializedOverride()
    {
        // Default implementation does nothing
    }

    /// <summary>
    /// Override for page-specific first render logic
    /// </summary>
    /// <returns>Async task</returns>
    protected virtual Task OnAfterFirstRenderAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Override for page-specific subsequent render logic
    /// </summary>
    /// <returns>Async task</returns>
    protected virtual Task OnAfterSubsequentRenderAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when render mode changes - override for custom transition handling
    /// </summary>
    /// <param name="oldMode">Previous render mode</param>
    /// <param name="newMode">New render mode</param>
    protected virtual void OnRenderModeChanged(string? oldMode, string newMode)
    {
        // Default implementation does nothing
    }

    /// <summary>
    /// Called when journey is updated - override for custom journey handling
    /// </summary>
    /// <param name="newState">Newly added render mode state</param>
    protected virtual void OnJourneyUpdated(RenderModeState newState)
    {
        // Default implementation does nothing
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the duration since component start in milliseconds
    /// </summary>
    /// <returns>Duration in milliseconds</returns>
    protected double GetDurationSinceStart()
    {
        return (DateTime.UtcNow - _startTime).TotalMilliseconds;
    }

    /// <summary>
    /// Gets the time to interactivity in milliseconds (if interactive)
    /// </summary>
    /// <returns>Time to interactivity or null if not interactive</returns>
    protected double? GetTimeToInteractive()
    {
        return _interactiveTime.HasValue ? (_interactiveTime.Value - _startTime).TotalMilliseconds : null;
    }

    /// <summary>
    /// Gets a basic transition time display for simple pages
    /// </summary>
    /// <returns>Formatted transition time string</returns>
    protected string GetBasicTransitionTime()
    {
        if (_interactiveTime.HasValue)
        {
            var actualMs = (_interactiveTime.Value - _startTime).TotalMilliseconds;
            return $"{actualMs.ToString("F0")}ms";
        }
        
        try
        {
            if (GetCurrentInteractive())
            {
                return "Measuring...";
            }
        }
        catch (InvalidOperationException)
        {
            // RendererInfo is not available in unit tests - this is expected
        }
        
        return "In progress...";
    }

    /// <summary>
    /// Gets the current render mode safely
    /// </summary>
    /// <returns>Current render mode or null if not available</returns>
    protected virtual string? GetCurrentRenderMode()
    {
        try
        {
            return RendererInfo.Name;
        }
        catch (InvalidOperationException)
        {
            // RendererInfo is not available in unit tests
            return "Test";
        }
    }

    /// <summary>
    /// Gets the current interactive status safely
    /// </summary>
    /// <returns>Whether component is currently interactive</returns>
    protected virtual bool GetCurrentInteractive()
    {
        try
        {
            return RendererInfo.IsInteractive;
        }
        catch (InvalidOperationException)
        {
            // RendererInfo is not available in unit tests
            return false;
        }
    }

    #endregion
}

/// <summary>
/// Represents a state in the render mode journey
/// </summary>
public class RenderModeState
{
    public string Mode { get; set; } = "";
    public string Duration { get; set; } = "";
} 