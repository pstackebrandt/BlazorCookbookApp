using Xunit;
using BlazorCookbookApp.Client.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorCookbookApp.Tests.Services;

/// <summary>
/// Tests for RecipeUrlService - URL parsing and title formatting functionality.
/// Ensures proper extraction of chapter/recipe numbers and title generation.
/// </summary>
public class RecipeUrlServiceTests
{
    [Theory]
    [InlineData("https://localhost:5001/ch01r02", 1, 2)]
    [InlineData("https://localhost:5001/ch01r04", 1, 4)]
    [InlineData("https://localhost:5001/ch05r12", 5, 12)]
    [InlineData("https://localhost:5001/ch10r01", 10, 1)]
    [InlineData("https://localhost:5001/ch01r03cl", 1, 3)]
    [InlineData("https://localhost:5001/ch01r04wademo", 1, 4)]
    public void Initialize_WithValidRecipeUrl_ExtractsChapterAndRecipeNumbers(string url, int expectedChapter, int expectedRecipe)
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = url };
        var service = new RecipeUrlService();

        // Act
        service.Initialize(navigationManager);

        // Assert
        Assert.Equal(expectedChapter, service.ChapterNumber);
        Assert.Equal(expectedRecipe, service.RecipeNumber);
    }

    [Theory]
    [InlineData("https://localhost:5001/")]
    [InlineData("https://localhost:5001/home")]
    [InlineData("https://localhost:5001/recipes")]
    [InlineData("https://localhost:5001/invalid")]
    [InlineData("https://localhost:5001/chapter01recipe02")]
    public void Initialize_WithNonRecipeUrl_SetsChapterAndRecipeToZero(string url)
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = url };
        var service = new RecipeUrlService();

        // Act
        service.Initialize(navigationManager);

        // Assert
        Assert.Equal(0, service.ChapterNumber);
        Assert.Equal(0, service.RecipeNumber);
    }

    [Theory]
    [InlineData(1, 2, "Chapter 1, Recipe 2")]
    [InlineData(5, 12, "Chapter 5, Recipe 12")]
    [InlineData(10, 1, "Chapter 10, Recipe 1")]
    [InlineData(1, 4, "Chapter 1, Recipe 4")]
    public void GetFormattedChapterRecipe_WithValidNumbers_ReturnsFormattedString(int chapter, int recipe, string expected)
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = $"https://localhost:5001/ch{chapter:D2}r{recipe:D2}" };
        var service = new RecipeUrlService();
        service.Initialize(navigationManager);

        // Act
        var result = service.GetFormattedChapterRecipe();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFormattedChapterRecipe_WithZeroNumbers_ReturnsUnknownMessage()
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = "https://localhost:5001/" };
        var service = new RecipeUrlService();
        service.Initialize(navigationManager);

        // Act
        var result = service.GetFormattedChapterRecipe();

        // Assert
        Assert.Equal("Unknown Chapter/Recipe", result);
    }

    [Theory]
    [InlineData("Server components", 1, 3, "Server components (Chapter 1, Recipe 3)")]
    [InlineData("Component parameters", 1, 5, "Component parameters (Chapter 1, Recipe 5)")]
    [InlineData("Render modes", 1, 4, "Render modes (Chapter 1, Recipe 4)")]
    [InlineData("", 1, 2, " (Chapter 1, Recipe 2)")]
    public void GetTitleWithNumbers_WithValidChapterRecipe_CombinesTitleAndNumbers(string baseTitle, int chapter, int recipe, string expected)
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = $"https://localhost:5001/ch{chapter:D2}r{recipe:D2}" };
        var service = new RecipeUrlService();
        service.Initialize(navigationManager);

        // Act
        var result = service.GetTitleWithNumbers(baseTitle);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetTitleWithNumbers_WithUnknownChapterRecipe_CombinesTitleWithUnknown()
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = "https://localhost:5001/" };
        var service = new RecipeUrlService();
        service.Initialize(navigationManager);

        // Act
        var result = service.GetTitleWithNumbers("Component data");

        // Assert
        Assert.Equal("Component data (Unknown Chapter/Recipe)", result);
    }

    [Fact]
    public void Initialize_WithComplexUrlPath_ExtractsRecipeNumbersCorrectly()
    {
        // Arrange
        var url = "https://localhost:5001/ch03r07/subpath?query=value";
        var navigationManager = new TestNavigationManager { Uri = url };
        var service = new RecipeUrlService();

        // Act
        service.Initialize(navigationManager);

        // Assert
        Assert.Equal(3, service.ChapterNumber);
        Assert.Equal(7, service.RecipeNumber);
    }

    [Fact]
    public void Initialize_CalledMultipleTimes_UpdatesValues()
    {
        // Arrange
        var navigationManager1 = new TestNavigationManager { Uri = "https://localhost:5001/ch01r02" };
        var navigationManager2 = new TestNavigationManager { Uri = "https://localhost:5001/ch03r05" };
        var service = new RecipeUrlService();
        
        // Act
        service.Initialize(navigationManager1);
        var firstChapter = service.ChapterNumber;
        var firstRecipe = service.RecipeNumber;
        
        service.Initialize(navigationManager2);

        // Assert
        Assert.Equal(1, firstChapter);
        Assert.Equal(2, firstRecipe);
        Assert.Equal(3, service.ChapterNumber);
        Assert.Equal(5, service.RecipeNumber);
    }

    [Theory]
    [InlineData("Server components")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Special characters: !@#$%")]
    [InlineData("Very long title that might be used in some scenarios")]
    public void GetTitleWithNumbers_WithVariousTitles_HandlesAllFormats(string title)
    {
        // Arrange
        var navigationManager = new TestNavigationManager { Uri = "https://localhost:5001/ch01r02" };
        var service = new RecipeUrlService();
        service.Initialize(navigationManager);

        // Act
        var result = service.GetTitleWithNumbers(title);

        // Assert
        Assert.Contains("(Chapter 1, Recipe 2)", result);
        Assert.StartsWith(title, result);
    }

    // Simple test implementation of NavigationManager
    private class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager() : base()
        {
        }

        public new string Uri
        {
            get => base.Uri;
            set => Initialize(value, value);
        }
    }
} 