using Microsoft.AspNetCore.Mvc;
using Moq;
using project_wildfire_web.Controllers;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using project_wildfire_web.ExtensionsMethods;

namespace project_wildfire_tests.UnitTests;

[TestFixture]
public class LocationApiControllerTests
{
    private LocationApiController _controller;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ILocationRepository> _locationRepositoryMock;
    private Mock<ILogger<WildfireAPIController>> _loggerMock;
    private Mock<UserManager<IdentityUser>> _userManagerMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();

        _loggerMock = new Mock<ILogger<WildfireAPIController>>();
        
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
    public async Task SaveLocation_WithValidPayload_ReturnsOk()
    {
        // Arrange
        var userLocation = new UserLocation
        {
            Latitude = 40.7128M,
            Longitude = -74.0060M
        };

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
        Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Expected OkObjectResult");
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo("Location saved successfully"), 
            "Expected success message");

    }

    [Test]
    public async Task SaveLocation_WithInvalidPayload_ReturnsError()
    {
        
    }
}