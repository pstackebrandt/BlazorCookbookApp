using BlazorCookbookApp.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace BlazorCookbookApp.Tests.Services;

/// <summary>
/// Tests for RecipeScanner - the core auto-discovery system.
/// Protects recipe pattern matching, summary extraction, and directory scanning logic.
/// </summary>
public class RecipeScannerTests
{
    private readonly RecipeScanner _scanner;

    public RecipeScannerTests()
    {
        var mockEnvironment = new MockWebHostEnvironment();
        _scanner = new RecipeScanner(mockEnvironment);
    }

    [Theory]
    [InlineData("@page \"/ch01r02\"", "/ch01r02", 1, 2, null)]
    [InlineData("@page \"/ch01r03cl\"", "/ch01r03cl", 1, 3, "cl")]
    [InlineData("@page \"/ch10r05\"", "/ch10r05", 10, 5, null)]
    [InlineData("@page \"/ch1r2\"", "/ch1r2", 1, 2, null)] // No leading zeros required
    [InlineData("@page \"/ch01r04wademo\"", "/ch01r04wademo", 1, 4, "wademo")]
    public void ParseRecipeFile_WithValidRoutePattern_ExtractsCorrectInfo(string content, string expectedRoute, int expectedChapter, int expectedRecipe, string expectedVariant)
    {
        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRoute, result.Route);
        Assert.Equal(expectedChapter, result.Chapter);
        Assert.Equal(expectedRecipe, result.Recipe);
        Assert.Equal(expectedVariant, result.Variant);
        Assert.Equal("Client", result.Location);
        Assert.Equal("/test/file.razor", result.FilePath);
    }

    [Theory]
    [InlineData("No route directive")]
    [InlineData("@page '/ch01r02'")]       // Single quotes instead of double
    [InlineData("@page \"/invalid\"")]     // Invalid pattern
    [InlineData("")]
    [InlineData("@page \"not-a-recipe\"")]
    public void ParseRecipeFile_WithInvalidRoutePattern_ReturnsNull(string content)
    {
        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseRecipeFile_WithCommentedRoute_StillMatches()
    {
        // Arrange - commented routes still match because regex doesn't check for line starts
        var content = "// @page \"/ch01r02\"";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert - the current implementation finds @page patterns even in comments
        Assert.NotNull(result);
        Assert.Equal("/ch01r02", result.Route);
        Assert.Equal(1, result.Chapter);
        Assert.Equal(2, result.Recipe);
    }

    [Theory]
    [InlineData("<h1>Simple Title</h1>", "Simple Title")]
    [InlineData("<h1>Multi\r\n                 line\r\n                 title</h1>", "Multi\r\n                 line\r\n                 title")]
    [InlineData("<h1><strong>Bold</strong> Title</h1>", "Bold Title")]
    [InlineData("<h1>Title with <span>nested</span> tags</h1>", "Title with nested tags")]
    [InlineData("<h2>H2 Title</h2>", "H2 Title")]
    public void ExtractSummary_WithValidH1Tag_ExtractsCleanText(string h1Content, string expectedSummary)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n{h1Content}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSummary, result.Summary);
    }

    [Theory]
    [InlineData("<h2>Fallback Title</h2>", "Fallback Title")]
    [InlineData("<h2>H2 with <em>emphasis</em></h2>", "H2 with emphasis")]
    public void ExtractSummary_WithNoH1ButH2_ExtractsH2Content(string h2Content, string expectedSummary)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n<p>Some content</p>\n{h2Content}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSummary, result.Summary);
    }

    [Fact]
    public void ExtractSummary_WithNoHeadingTags_ReturnsDefaultMessage()
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n<p>Just paragraph content</p>";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("No summary available", result.Summary);
    }

    [Theory]
    [InlineData("Server")]
    [InlineData("Client")]
    public void ParseRecipeFile_WithDifferentLocations_SetsLocationCorrectly(string location)
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n<h1>Test</h1>";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", location);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(location, result.Location);
    }

    [Theory]
    [InlineData("@page \"/ch01r02\"\n<h1>Test</h1>", true)]
    [InlineData("@page \"/invalid\"\n<h1>Test</h1>", false)]
    [InlineData("No valid content", false)]
    public void ParseRecipeFile_WithVariousContent_ReturnsExpectedResult(string content, bool shouldReturnResult)
    {
        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        if (shouldReturnResult)
        {
            Assert.NotNull(result);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Fact]
    public void ParseRecipeFile_WithComplexContent_ExtractsCorrectly()
    {
        // Arrange
        var content = @"
            @page ""/ch02r05custom""
            @using SomeNamespace
            
            <h1>Complex Recipe Title</h1>
            <p>Some description content</p>
            <h2>Secondary heading</h2>
        ";

        // Act
        var result = ParseRecipeFile(content, "/complex/file.razor", "Server");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("/ch02r05custom", result.Route);
        Assert.Equal(2, result.Chapter);
        Assert.Equal(5, result.Recipe);
        Assert.Equal("custom", result.Variant);
        Assert.Equal("Server", result.Location);
        Assert.Equal("Complex Recipe Title", result.Summary);
    }

    // Helper method to access private ParseRecipeFile method using reflection
    private RecipeInfo? ParseRecipeFile(string content, string filePath, string location)
    {
        var method = typeof(RecipeScanner).GetMethod("ParseRecipeFile", BindingFlags.NonPublic | BindingFlags.Instance);
        return (RecipeInfo?)method?.Invoke(_scanner, new object[] { content, filePath, location });
    }

    // Mock implementation for testing
    private class MockWebHostEnvironment : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = "";
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public string ApplicationName { get; set; } = "Test";
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
        public string ContentRootPath { get; set; } = "/test";
        public string EnvironmentName { get; set; } = "Test";
    }
} 