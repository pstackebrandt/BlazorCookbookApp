using Microsoft.AspNetCore.Components;

namespace BlazorCookbookApp.Tests.Shared;

/// <summary>
/// Unit tests for RenderModeComponentBase class
/// Tests the common functionality that will be extracted from render mode pages
/// </summary>
public class RenderModeComponentBaseTests
{
    /// <summary>
    /// Test implementation of RenderModeComponentBase for testing purposes
    /// </summary>
    private class TestRenderModeComponent : RenderModeComponentBase
    {
        protected override string PageTitle => "Test Render Mode";
        
        // Expose protected methods for testing
        public new string GetDisplayRenderMode() => base.GetDisplayRenderMode();
        public new bool GetDisplayInteractive() => base.GetDisplayInteractive();
        public new string GetRenderModeClass() => base.GetRenderModeClass();
        
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
        
        // Mock RendererInfo for testing
        public string? MockRendererName { get; set; } = "Static";
        public bool MockIsInteractive { get; set; } = false;
        
        // Override the RendererInfo access (we'll need to implement this in the base class)
        protected override string? GetCurrentRenderMode() => MockRendererName;
        protected override bool GetCurrentInteractive() => MockIsInteractive;
    }

    [Fact]
    public void GetDisplayRenderMode_WhenDelayed_ReturnsStatic()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = true;
        component.MockRendererName = "WebAssembly";

        // Act
        var result = component.GetDisplayRenderMode();

        // Assert
        Assert.Equal("Static", result);
    }

    [Fact]
    public void GetDisplayRenderMode_WhenNotDelayed_ReturnsActualMode()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockRendererName = "WebAssembly";

        // Act
        var result = component.GetDisplayRenderMode();

        // Assert
        Assert.Equal("WebAssembly", result);
    }

    [Fact]
    public void GetDisplayRenderMode_WhenNotDelayedAndNoMode_ReturnsUnknown()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockRendererName = null;

        // Act
        var result = component.GetDisplayRenderMode();

        // Assert
        Assert.Equal("Unknown", result);
    }

    [Fact]
    public void GetDisplayInteractive_WhenDelayed_ReturnsFalse()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = true;
        component.MockIsInteractive = true;

        // Act
        var result = component.GetDisplayInteractive();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetDisplayInteractive_WhenNotDelayedAndInteractive_ReturnsTrue()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockIsInteractive = true;

        // Act
        var result = component.GetDisplayInteractive();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetDisplayInteractive_WhenNotDelayedAndNotInteractive_ReturnsFalse()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockIsInteractive = false;

        // Act
        var result = component.GetDisplayInteractive();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("webassembly", "bg-success text-white")]
    [InlineData("WebAssembly", "bg-success text-white")]
    [InlineData("server", "bg-primary text-white")]
    [InlineData("Server", "bg-primary text-white")]
    [InlineData("static", "bg-warning text-dark")]
    [InlineData("Static", "bg-warning text-dark")]
    [InlineData("unknown", "bg-secondary text-white")]
    [InlineData("", "bg-secondary text-white")]
    public void GetRenderModeClass_ReturnsCorrectCssClasses(string? renderMode, string expectedClass)
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockRendererName = renderMode;

        // Act
        var result = component.GetRenderModeClass();

        // Assert
        Assert.Equal(expectedClass, result);
    }

    [Fact]
    public void GetRenderModeClass_WhenDelayed_ReturnsStaticClass()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = true;
        component.MockRendererName = "WebAssembly"; // Should be ignored when delayed

        // Act
        var result = component.GetRenderModeClass();

        // Assert
        Assert.Equal("bg-warning text-dark", result); // Static class
    }

    [Fact]
    public void GetRenderModeClass_WhenRenderModeIsNull_ReturnsSecondaryClass()
    {
        // Arrange
        var component = new TestRenderModeComponent();
        component._isDelayed = false;
        component.MockRendererName = null;

        // Act
        var result = component.GetRenderModeClass();

        // Assert
        Assert.Equal("bg-secondary text-white", result);
    }

    [Fact]
    public void StaticPhaseDelayMs_HasCorrectValue()
    {
        // Arrange & Act
        var component = new TestRenderModeComponent();

        // Assert
        Assert.Equal(1500, TestRenderModeComponent.STATIC_PHASE_DELAY_MS);
    }

    [Fact]
    public void Constructor_InitializesFieldsCorrectly()
    {
        // Arrange & Act
        var component = new TestRenderModeComponent();

        // Assert
        Assert.True(component._isDelayed);
        Assert.True(component._startTime > DateTime.MinValue);
        Assert.Null(component._interactiveTime);
    }
}

/// <summary>
/// Placeholder for the base class that will be implemented in T8.1.1
/// This allows us to write tests first and implement the class to satisfy them
/// </summary>
public abstract class RenderModeComponentBase : ComponentBase
{
    protected bool _isDelayed = true;
    protected DateTime _startTime = DateTime.UtcNow;
    protected DateTime? _interactiveTime = null;
    public const int STATIC_PHASE_DELAY_MS = 1500;

    protected abstract string PageTitle { get; }

    protected virtual string GetDisplayRenderMode()
    {
        return _isDelayed ? "Static" : (GetCurrentRenderMode() ?? "Unknown");
    }

    protected virtual bool GetDisplayInteractive()
    {
        return !_isDelayed && GetCurrentInteractive();
    }

    protected virtual string GetRenderModeClass()
    {
        var mode = GetDisplayRenderMode();
        return mode?.ToLower() switch
        {
            "webassembly" => "bg-success text-white",    // Green for WebAssembly
            "server" => "bg-primary text-white",         // Blue for Server
            "static" => "bg-warning text-dark",          // Yellow for Static
            _ => "bg-secondary text-white"               // Gray for unknown
        };
    }

    // Abstract methods for accessing RendererInfo (will be implemented properly in the actual base class)
    protected abstract string? GetCurrentRenderMode();
    protected abstract bool GetCurrentInteractive();
} 