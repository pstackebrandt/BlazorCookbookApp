using BlazorCookbookApp.Services;
using Xunit;

namespace BlazorCookbookApp.Tests.Services;

/// <summary>
/// Tests for RecipeInfo data model.
/// Ensures data integrity and proper handling of various input scenarios.
/// </summary>
public class RecipeInfoTests
{
    [Fact]
    public void Constructor_InitializesWithDefaultValues()
    {
        // Act
        var recipe = new RecipeInfo();

        // Assert
        Assert.Equal(string.Empty, recipe.Route);
        Assert.Equal(0, recipe.Chapter);
        Assert.Equal(0, recipe.Recipe);
        Assert.Null(recipe.Variant);
        Assert.Equal(string.Empty, recipe.Location);
        Assert.Equal(string.Empty, recipe.Summary);
        Assert.Equal(string.Empty, recipe.FilePath);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Route = "/ch01r04";
        recipe.Chapter = 1;
        recipe.Recipe = 4;
        recipe.Variant = "wademo";
        recipe.Location = "Client";
        recipe.Summary = "Test summary";
        recipe.FilePath = "/path/to/file.razor";

        // Assert
        Assert.Equal("/ch01r04", recipe.Route);
        Assert.Equal(1, recipe.Chapter);
        Assert.Equal(4, recipe.Recipe);
        Assert.Equal("wademo", recipe.Variant);
        Assert.Equal("Client", recipe.Location);
        Assert.Equal("Test summary", recipe.Summary);
        Assert.Equal("/path/to/file.razor", recipe.FilePath);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("cl")]
    [InlineData("s")]
    [InlineData("a")]
    [InlineData("wademo")]
    public void Variant_HandlesNullAndVariousValues(string? variant)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Variant = variant;

        // Assert
        Assert.Equal(variant, recipe.Variant);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("No summary available")]
    [InlineData("Recipe with <em>HTML</em> tags")]
    [InlineData("Very long summary that might be truncated in some display scenarios but should be preserved in the data model")]
    public void Summary_HandlesVariousStringValues(string summary)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Summary = summary;

        // Assert
        Assert.Equal(summary, recipe.Summary);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99)]
    public void Chapter_HandlesValidNumbers(int chapter)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Chapter = chapter;

        // Assert
        Assert.Equal(chapter, recipe.Chapter);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99)]
    public void Recipe_HandlesValidNumbers(int recipeNumber)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Recipe = recipeNumber;

        // Assert
        Assert.Equal(recipeNumber, recipe.Recipe);
    }

    [Theory]
    [InlineData("Server")]
    [InlineData("Client")]
    [InlineData("Shared")]
    [InlineData("")]
    public void Location_HandlesVariousLocationValues(string location)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Location = location;

        // Assert
        Assert.Equal(location, recipe.Location);
    }

    [Theory]
    [InlineData("/ch01r02")]
    [InlineData("/ch01r03cl")]
    [InlineData("/ch01r04wademo")]
    [InlineData("")]
    [InlineData("/custom/route")]
    public void Route_HandlesVariousRouteFormats(string route)
    {
        // Arrange
        var recipe = new RecipeInfo();

        // Act
        recipe.Route = route;

        // Assert
        Assert.Equal(route, recipe.Route);
    }

    [Fact]
    public void RecipeInfo_SupportsCompleteRecipeData()
    {
        // Act
        var recipe = new RecipeInfo
        {
            Route = "/ch01r04wademo",
            Chapter = 1,
            Recipe = 4,
            Variant = "wademo",
            Location = "Client",
            Summary = "Interactive WebAssembly Features Demo",
            FilePath = "/Users/dev/BlazorCookbook/Client/Pages/Recipe4/WebAssemblyDemo.razor"
        };

        // Assert - All properties should be correctly set
        Assert.Equal("/ch01r04wademo", recipe.Route);
        Assert.Equal(1, recipe.Chapter);
        Assert.Equal(4, recipe.Recipe);
        Assert.Equal("wademo", recipe.Variant);
        Assert.Equal("Client", recipe.Location);
        Assert.Equal("Interactive WebAssembly Features Demo", recipe.Summary);
        Assert.Equal("/Users/dev/BlazorCookbook/Client/Pages/Recipe4/WebAssemblyDemo.razor", recipe.FilePath);
    }
} 