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
        Assert.Equal("file", result.FilePath);
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
    [InlineData("public string PageSummary { get; set; } = \"Simple Title\";", "Simple Title")]
    [InlineData("public string PageSummary { get; set; } = \"Multi line title\";", "Multi line title")]
    [InlineData("protected override string PageSummary => \"Override Title\";", "Override Title")]
    [InlineData("public string PageSummary { get; set; } = \"Title with nested tags\";", "Title with nested tags")]
    public void ExtractSummary_WithValidPageSummaryProperty_ExtractsCorrectValue(string propertyContent, string expectedSummary)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n@code {{\n{propertyContent}\n}}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSummary, result.Summary);
    }

    [Theory]
    [InlineData("public string PageTitle { get; set; } = \"Fallback Title\";", "Fallback Title")]
    [InlineData("protected override string PageTitle => \"Override Title\";", "Override Title")]
    public void ExtractTitle_WithValidPageTitleProperty_ExtractsCorrectValue(string propertyContent, string expectedTitle)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n@code {{\n{propertyContent}\n}}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTitle, result.Title);
    }

    [Fact]
    public void ExtractSummary_WithNoPageSummaryProperty_ReturnsUnknown()
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n<p>Just paragraph content</p>";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("unknown", result.Summary);
    }

    [Fact]
    public void ExtractTitle_WithNoPageTitleProperty_ReturnsUnknown()
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n<p>Just paragraph content</p>";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("unknown", result.Title);
    }

    [Theory]
    [InlineData("Server")]
    [InlineData("Client")]
    public void ParseRecipeFile_WithDifferentLocations_SetsLocationCorrectly(string location)
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n@code { public string PageTitle { get; set; } = \"Test\"; }";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", location);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(location, result.Location);
    }

    [Theory]
    [InlineData("@page \"/ch01r02\"\n@code { public string PageTitle { get; set; } = \"Test\"; }", true)]
    [InlineData("@page \"/invalid\"\n@code { public string PageTitle { get; set; } = \"Test\"; }", false)]
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
            
            @code {
                public string PageTitle { get; set; } = ""Complex Recipe Title"";
                public string PageSummary { get; set; } = ""Complex recipe summary"";
                public int PageStars { get; set; } = 4;
            }
            
            <p>Some description content</p>
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
        Assert.Equal("Complex Recipe Title", result.Title);
        Assert.Equal("Complex recipe summary", result.Summary);
        Assert.Equal(4, result.Stars);
        Assert.Equal("file", result.FilePath);
    }

    [Theory]
    [InlineData("public int PageStars { get; set; } = 5;", 5)]
    [InlineData("protected override int PageStars => 4;", 4)]
    [InlineData("public int PageStars { get; set; } = 1;", 1)]
    [InlineData("public int PageStars { get; set; } = 3;", 3)]
    public void ExtractStars_WithValidPageStarsProperty_ExtractsCorrectValue(string propertyContent, int expectedStars)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n@code {{\n{propertyContent}\n}}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStars, result.Stars);
    }

    [Fact]
    public void ExtractStars_WithNoPageStarsProperty_ReturnsDefault()
    {
        // Arrange
        var content = "@page \"/ch01r02\"\n<p>Just paragraph content</p>";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Stars); // Default value
    }

    [Theory]
    [InlineData("public int PageStars { get; set; } = 0;", 3)] // Out of range, should default to 3
    [InlineData("public int PageStars { get; set; } = 6;", 3)] // Out of range, should default to 3
    [InlineData("public int PageStars { get; set; } = -1;", 3)] // Out of range, should default to 3
    public void ExtractStars_WithInvalidPageStarsValue_ReturnsDefault(string propertyContent, int expectedStars)
    {
        // Arrange
        var content = $"@page \"/ch01r02\"\n@code {{\n{propertyContent}\n}}";

        // Act
        var result = ParseRecipeFile(content, "/test/file.razor", "Client");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStars, result.Stars);
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