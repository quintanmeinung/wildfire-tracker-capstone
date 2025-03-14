using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project_wildfire_web.Models;
using project_wildfire_web.Services;

public class UserPreferencesServiceTests
{
    private readonly Mock<FireDataDbContext> _mockDbContext;
    private readonly Mock<DbSet<UserPreferences>> _mockDbSet;
    private readonly UserPreferencesService _preferencesService;
    private readonly Mock<ILogger<UserPreferencesService>> _mockLogger;

    public UserPreferencesServiceTests()
    {
        _mockDbContext = new Mock<FireDataDbContext>();
        _mockDbSet = new Mock<DbSet<UserPreferences>>();
        _mockLogger = new Mock<ILogger<UserPreferencesService>>();

        // Setup DbContext to use mocked DbSet
        _mockDbContext.Setup(db => db.UserPreferences).Returns(_mockDbSet.Object);

        // Initialize the service with the mocked DbContext
        _preferencesService = new UserPreferencesService(_mockDbContext.Object);
    }

    [Fact]
    public async Task GetPreferences_ReturnsUserPreferences_WhenUserExists()
    {
        // Arrange
        var userId = "user123";
        var expectedPreferences = new UserPreferences { UserId = userId, FontSize = "16px", ContrastMode = true, TextToSpeech = false };

        _mockDbSet.Setup(db => db.FindAsync(userId)).ReturnsAsync(expectedPreferences);

        // Act
        var result = await _preferencesService.GetPreferences(userId);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("16px", result.FontSize);
        Xunit.Assert.True(result.ContrastMode);
        Xunit.Assert.False(result.TextToSpeech);
    }

    [Fact]
    public async Task GetPreferences_ReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "nonexistent_user";

        _mockDbSet.Setup(db => db.FindAsync(userId)).ReturnsAsync((UserPreferences)null);

        // Act
        var result = await _preferencesService.GetPreferences(userId);

        // Assert
        Xunit.Assert.Null(result);
    }

    [Fact]
    public async Task SavePreferences_AddsNewUserPreferences_WhenNoExistingPreferences()
    {
        // Arrange
        var userId = "new_user";
        var newPreferences = new UserPreferences { UserId = userId, FontSize = "18px", ContrastMode = false, TextToSpeech = true };

        _mockDbSet.Setup(db => db.FindAsync(userId)).ReturnsAsync((UserPreferences)null);

        // Act
        await _preferencesService.SavePreferences(newPreferences);

        // Assert
        _mockDbSet.Verify(db => db.Add(It.IsAny<UserPreferences>()), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SavePreferences_UpdatesExistingUserPreferences()
    {
        // Arrange
        var userId = "existing_user";
        var existingPreferences = new UserPreferences { UserId = userId, FontSize = "14px", ContrastMode = false, TextToSpeech = false };
        var updatedPreferences = new UserPreferences { UserId = userId, FontSize = "20px", ContrastMode = true, TextToSpeech = true };

        _mockDbSet.Setup(db => db.FindAsync(userId)).ReturnsAsync(existingPreferences);

        // Act
        await _preferencesService.SavePreferences(updatedPreferences);

        // Assert
        Xunit.Assert.Equal("20px", existingPreferences.FontSize);
        Xunit.Assert.True(existingPreferences.ContrastMode);
        Xunit.Assert.True(existingPreferences.TextToSpeech);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
}
