using Microsoft.AspNetCore.Components;
using BlazorCookbookApp.Client.Shared;

namespace BlazorCookbookApp.Tests.Shared;

/// <summary>
/// Unit tests for RenderModeComponentBase class
/// Tests the common functionality extracted from render mode pages
/// </summary>
public class RenderModeComponentBaseTests
{
    /// <summary>
    /// Test implementation of RenderModeComponentBase for testing purposes
    /// </summary>
    private class TestRenderModeComponent : RenderModeComponentBase
    {
        protected override string PageTitle => "Test Render Mode";
        protected override string PageSummary => "Test render mode summary";
        
        // Expose protected methods for testing
        public new string GetDisplayRenderMode() => base.GetDisplayRenderMode();
        public new bool GetDisplayInteractive() => base.GetDisplayInteractive();
        public new string GetRenderModeClass() => base.GetRenderModeClass();
        public string GetRenderModeClassWithParameter(string renderMode) => base.GetRenderModeClass(renderMode);
        
        // Expose protected fields for testing
        public new bool _isDelayed 
        { 
            get => base._isDelayed; 
            set => base._isDelayed = value; 
        }
        
        public new DateTime _startTime 
        { 
            get => base._startTime; 
            set => base._startTime = value; 
        }
        
        public new DateTime? _interactiveTime 
        { 
            get => base._interactiveTime; 
            set => base._interactiveTime = value; 
        }

        // Expose action history methods for testing
        public new void AddAction(string description, RenderActionCategory category) => base.AddAction(description, category);
        public new Dictionary<string, List<RenderAction>> GetActionsByCategory() => base.GetActionsByCategory();
        public new bool HasActionHistory => base.HasActionHistory;
        public new List<RenderAction> _actionHistory 
        { 
            get => base._actionHistory; 
            set => base._actionHistory = value; 
        }

        // Expose helper methods for testing
        public double GetDurationSinceStartPublic() => base.GetDurationSinceStart();
        public double? GetTimeToInteractivePublic() => base.GetTimeToInteractive();
        public string GetBasicTransitionTimePublic() => base.GetBasicTransitionTime();
    }

    [Fact]
    public void GetDisplayRenderMode_WhenDelayed_ReturnsActualRenderMode()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = true;

        // Act
        var result = component.GetDisplayRenderMode();

        // Assert
        Assert.Equal("Test", result); // Expect actual render mode, not "Static"
    }



    [Theory]
    [InlineData("webassembly", "bg-success text-white")]
    [InlineData("server", "bg-success text-white")]
    [InlineData("static", "bg-warning text-dark")]
    [InlineData("unknown", "bg-secondary text-white")]
    public void GetRenderModeClass_WithSpecificRenderMode_ReturnsCorrectCssClasses(string renderMode, string expectedClass)
    {
        // Arrange
        var component = new TestRenderModeComponent();

        // Act
        var result = component.GetRenderModeClassWithParameter(renderMode);

        // Assert
        Assert.Equal(expectedClass, result);
    }

    [Fact]
    public void Constructor_InitializesFieldsCorrectly()
    {
        // Arrange & Act
        var component = new TestRenderModeComponent();

        // Assert
        Assert.True(component._isDelayed); // Should start delayed
        Assert.Null(component._interactiveTime); // Should start null
        Assert.Empty(component._actionHistory); // Should start empty
        Assert.False(component.HasActionHistory); // Should be false when empty
    }

    [Fact]
    public void AddAction_AddsActionToHistory()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._startTime = DateTime.UtcNow.AddMilliseconds(-100); // 100ms ago

        // Act
        component.AddAction("Test action", RenderActionCategory.Initialization);

        // Assert
        Assert.Single(component._actionHistory);
        Assert.True(component.HasActionHistory);
        
        var action = component._actionHistory.First();
        Assert.Equal("Test action", action.Description);
        Assert.Equal(RenderActionCategory.Initialization, action.Category);
        Assert.True(action.DurationMs >= 90); // Should be at least 90ms
        Assert.NotEmpty(action.Time);
    }

    [Fact]
    public void GetActionsByCategory_GroupsActionsCorrectly()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component.AddAction("Init 1", RenderActionCategory.Initialization);
        component.AddAction("Init 2", RenderActionCategory.Initialization);
        component.AddAction("Transition 1", RenderActionCategory.Transition);

        // Act
        var result = component.GetActionsByCategory();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("Initialization", result.Keys);
        Assert.Contains("Transition", result.Keys);
        Assert.Equal(2, result["Initialization"].Count);
        Assert.Single(result["Transition"]);
    }

    [Fact]
    public void GetBasicTransitionTime_WhenInteractiveTimeIsNull_ReturnsInProgress()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._interactiveTime = null;

        // Act
        var result = component.GetBasicTransitionTimePublic();

        // Assert
        Assert.Equal("In progress...", result);
    }

    [Fact]
    public void GetBasicTransitionTime_WhenInteractiveTimeIsSet_ReturnsFormattedTime()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        var startTime = DateTime.UtcNow;
        var interactiveTime = startTime.AddMilliseconds(150);
        
        component._startTime = startTime;
        component._interactiveTime = interactiveTime;

        // Act
        var result = component.GetBasicTransitionTimePublic();

        // Assert
        Assert.Equal("150ms", result);
    }





    [Fact]
    public void RenderActionCategory_HasAllExpectedValues()
    {
        // Arrange & Act & Assert
        var categories = Enum.GetValues<RenderActionCategory>();
        
        Assert.Contains(RenderActionCategory.Initialization, categories);
        Assert.Contains(RenderActionCategory.Transition, categories);
        Assert.Contains(RenderActionCategory.Active, categories);
        Assert.Contains(RenderActionCategory.Interaction, categories);
        Assert.Contains(RenderActionCategory.ServerPhase, categories);
        Assert.Contains(RenderActionCategory.ClientTransition, categories);
        Assert.Contains(RenderActionCategory.ClientActive, categories);
    }

    [Fact]
    public void RenderAction_HasAllRequiredProperties()
    {
        // Arrange & Act
        var action = new RenderAction
        {
            Time = "12:34:56.789",
            DurationMs = 123.45,
            Description = "Test description",
            Category = RenderActionCategory.Initialization,
            RenderMode = "WebAssembly"
        };

        // Assert
        Assert.Equal("12:34:56.789", action.Time);
        Assert.Equal(123.45, action.DurationMs);
        Assert.Equal("Test description", action.Description);
        Assert.Equal(RenderActionCategory.Initialization, action.Category);
        Assert.Equal("WebAssembly", action.RenderMode);
    }
} 