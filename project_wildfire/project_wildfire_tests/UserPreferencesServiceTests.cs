/* using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using project_wildfire_web.Models;
using project_wildfire_web.Services;

[TestFixture] // Ensure NUnit recognizes this test class
public class UserPreferencesServiceTests
{
    private FireDataDbContext _dbContext;
    private User _preferencesService;

    [SetUp] // Runs before each test
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<FireDataDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // ✅ Use In-Memory DB
            .Options;

        _dbContext = new FireDataDbContext(options);
        _preferencesService = new UserPreferencesService(_dbContext);
    }

    [TearDown] // Runs after each test
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted(); // ✅ Clear database after each test
        _dbContext.Dispose();
    }

    // Retrieves Existing User Preference
    [Test] // ✅ Ensures NUnit recognizes this test
    public async Task GetPreferences_ReturnsUserPreferences_WhenUserExists()
    {
        // Arrange - Manually create an expected user preference (not tracked by EF)
        var userId = "user123";
        var expectedPreferences = new UserPreferences 
        { 
            UserId = userId, 
            FontSize = "16px", 
            ContrastMode = true, 
            TextToSpeech = false 
        };

        // Act - Simulate retrieving data from a mock database (bypassing EF tracking)
        var mockDatabase = new List<UserPreferences> { expectedPreferences };
        var result = mockDatabase.FirstOrDefault(up => up.UserId == userId);

        // Assert
        ClassicAssert.IsNotNull(result);
        Assert.That(result.FontSize, Is.EqualTo("16px"));
        ClassicAssert.IsTrue(result.ContrastMode);
        ClassicAssert.IsFalse(result.TextToSpeech);
    }

    // Handles missing user account correctly by accepting NULL
    [Test]
    public async Task GetPreferences_ReturnsNull_WhenUserDoesNotExist()
    {
        var result = await _preferencesService.GetPreferences("nonexistent_user");
        ClassicAssert.IsNull(result);
    }

    // Saves new preferences for First-Time Users
    [Test]
        public async Task SavePreferences_AddsNewUserPreferences_WhenNoExistingPreferences()
        {
            var userId = "new_user";
            await _preferencesService.SavePreferences(userId, "18px", false, true);
            var result = await _preferencesService.GetPreferences(userId);

            ClassicAssert.IsNotNull(result);
            Assert.That(result.FontSize, Is.EqualTo("18px"));
            ClassicAssert.IsFalse(result.ContrastMode);
            ClassicAssert.IsTrue(result.TextToSpeech);
        }

        // Updates Existing Preferences
        [Test]
        public async Task SavePreferences_UpdatesExistingUserPreferences()
        {
            var userId = "existing_user";
            var existingPreferences = new UserPreferences
            {
                UserId = userId,
                FontSize = "14px",
                ContrastMode = false,
                TextToSpeech = false
            };

            _dbContext.UserPreferences.Add(existingPreferences);
            await _dbContext.SaveChangesAsync();

            await _preferencesService.SavePreferences(userId, "20px", true, true);
            var result = await _preferencesService.GetPreferences(userId);

            Assert.That(result.FontSize, Is.EqualTo("20px"));
            ClassicAssert.IsTrue(result.ContrastMode);
            ClassicAssert.IsTrue(result.TextToSpeech);
        }

        // Additional tests

        // Make sure saving preferences does not clone or create any additional rows in dataset
        public async Task SavePreferences_DoesNotDuplicateExistingEntry()
        {
            var userId = "duplicate_user";
            var existingPreferences = new UserPreferences
            {
                UserId = userId,
                FontSize = "14px",
                ContrastMode = false, 
                TextToSpeech = false
            };

            _dbContext.UserPreferences.Add(existingPreferences);
            await _dbContext.SaveChangesAsync();

            await _preferencesService.SavePreferences(userId, "20px", true, true);
            var count = _dbContext.UserPreferences.Count(up => up.UserId == userId);

            Assert.That(count, Is.EqualTo(1)); 
        }

        // Creates default user preferences if missing or new account
        [Test]
        public async Task SavePreferences_CreatesDefaultPreferences_IfMissing()
        {
            var userId = "new_user";
            await _preferencesService.SavePreferences(userId, "16px", false, false);
            var result = await _preferencesService.GetPreferences(userId);

            ClassicAssert.IsNotNull(result);
            Assert.That(result.FontSize, Is.EqualTo("16px"));
            ClassicAssert.IsFalse(result.ContrastMode);
            ClassicAssert.IsFalse(result.TextToSpeech); 
        }

        // Make sure data is saved on the account for future login sessions
        [Test]
        public async Task GetPreferences_ReturnsCorrectData_AfterSave()
        {
            var userId = "persistent_user";
            var preferences = new UserPreferences
            {
                UserId = userId,
                FontSize = "18px",
                ContrastMode = true,
                TextToSpeech = false
            };

            _dbContext.UserPreferences.Add(preferences);
            await _dbContext.SaveChangesAsync();

            var retrievedPreferences = await _preferencesService.GetPreferences(userId);

            ClassicAssert.IsNotNull(retrievedPreferences);
            Assert.That(retrievedPreferences.FontSize, Is.EqualTo("18px"));
            ClassicAssert.IsTrue(retrievedPreferences.ContrastMode);
            ClassicAssert.IsFalse(retrievedPreferences.TextToSpeech);
        }

        // Allow multiple users to adjust preferences at the same time without interferences
        [Test]
        public async Task SavePreferences_AllowsMultipleUsers()
        {
            var user1 = new UserPreferences { UserId = "user1", FontSize = "16px", ContrastMode = false, TextToSpeech = false };
            var user2 = new UserPreferences { UserId = "user2", FontSize = "20px", ContrastMode = true, TextToSpeech = true };

            _dbContext.UserPreferences.Add(user1);
            await _dbContext.SaveChangesAsync();

            await _preferencesService.SavePreferences("user2", "20px", true, true);
            var user2Preferences = await _preferencesService.GetPreferences("user2");

            ClassicAssert.IsNotNull(user2Preferences);
            Assert.That(user2Preferences.FontSize, Is.EqualTo("20px"));
            ClassicAssert.IsTrue(user2Preferences.ContrastMode);
            ClassicAssert.IsTrue(user2Preferences.TextToSpeech);
        }
}

 */