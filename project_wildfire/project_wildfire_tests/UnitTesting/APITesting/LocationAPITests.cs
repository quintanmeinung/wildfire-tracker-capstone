using Microsoft.AspNetCore.Mvc;
using Moq;
using project_wildfire_web.Controllers;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using project_wildfire_web.ExtensionsMethods;
using FluentAssertions.Specialized;
using System.Text.Json.Nodes;
using project_wildfire_web.Models.DTO;

namespace project_wildfire_tests.UnitTests;

[TestFixture]
public class LocationApiControllerTests
{   
    // All of these mocks are necessary for the UserManager.
    private LocationApiController _controller;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ILocationRepository> _locationRepositoryMock;
    private Mock<ILogger<LocationApiController>> _loggerMock;
    private Mock<UserManager<IdentityUser>> _userManagerMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();

        _loggerMock = new Mock<ILogger<LocationApiController>>();
        
        // UserManager is abstract, so we need a special setup for it
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        
        #pragma warning disable CS8625 //Suppress warnings for null parameters
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        #pragma warning restore CS8625 //Restore warnings for null parameters
        
        _controller = new LocationApiController(
            _loggerMock.Object,
            _userManagerMock.Object,
            _userRepositoryMock.Object,
            _locationRepositoryMock.Object);
    }

    [Test]
    public async Task SaveLocation_WithValidModelState_ReturnsOk()
    {
        // Arrange
        var userLocation = CreateTestUserLocation();

        // Mock UserManager to return a user
        var testUser = new IdentityUser { Id = userLocation.UserId };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(testUser);

        _locationRepositoryMock.Setup(repo => repo.AddLocationAsync(It.IsAny<UserLocation>()))
            .Returns(Task.CompletedTask)
            .Verifiable();  // Allows us to verify this was called

        // Act
        var result = await _controller.SaveLocation(userLocation.ToUserLocationDTO());

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkResult");
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo("Location saved successfully"), 
            "Expected success message");

    }

    [Test]
    public async Task SaveLocation_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var userLocation = CreateTestUserLocation();
        _controller.ModelState.AddModelError("Field", "Error message");

        // Act
        var result = await _controller.SaveLocation(userLocation.ToUserLocationDTO());

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task SaveLocation_WhenRepositoryFails_ReturnsServerError()
    {
        // Arrange
        var userLocation = CreateTestUserLocation();
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(new IdentityUser());
        
        _locationRepositoryMock.Setup(repo => repo.AddLocationAsync(It.IsAny<UserLocation>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.SaveLocation(userLocation.ToUserLocationDTO());

        // Assert
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task DeleteLocation_WithValidId_ReturnsOk()
    {
        // Arrange
        var locationId = "test-location-id";
        _locationRepositoryMock.Setup(repo => repo.DeleteLocationAsync(locationId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteLocation(locationId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkResult");
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo("Location deleted" ), "Expected success message");
        _locationRepositoryMock.Verify();
    }

    [Test]
    public async Task DeleteLocation_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        string locationId = ""; // Invalid ID

        // Act
        var result = await _controller.DeleteLocation(locationId);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>(), "Expected BadRequestResult");
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.EqualTo("LocationId is required"), 
            "Expected error message for null ID");
    }

    [Test]
    public async Task UpdateLocation_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var invalidDto = new UserLocationDTO();
        _controller.ModelState.AddModelError("UserId", "UserId is required");

        // Act
        var result = await _controller.UpdateLocation(invalidDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

     [Test]
    public async Task UpdateLocation_WithNullUserId_ReturnsBadRequest()
    {
        // Arrange
        var location = CreateTestUserLocation();
        var dto = location.ToUserLocationDTO();
        dto.UserId = null; // Simulate null UserId

        // Act
        var result = await _controller.UpdateLocation(dto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateLocation_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var location = CreateTestUserLocation();
        var dto = location.ToUserLocationDTO();

        _userManagerMock.Setup(um => um.FindByIdAsync(dto.UserId))
            .ReturnsAsync((IdentityUser)null); // Simulate user not found

        // Act
        var result = await _controller.UpdateLocation(dto); 

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdateLocation_WithValidModel_ReturnsOk()
    {
        // Arrange
        var location = CreateTestUserLocation();
        var dto = location.ToUserLocationDTO();

        var testUser = new IdentityUser { Id = dto.UserId };
        _userManagerMock.Setup(um => um.FindByIdAsync(dto.UserId))
            .ReturnsAsync(testUser);

        _locationRepositoryMock.Setup(repo => repo.UpdateLocationAsync(It.IsAny<UserLocation>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdateLocation(dto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    private static UserLocation CreateTestUserLocation()
    {
        return new UserLocation
        {
            UserId = "test-user-id",
            Latitude = 40.7128M,
            Longitude = -74.0060M
        };
    }
}