/*using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using project_wildfire_web.Controllers;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.Services;

[TestFixture] 
public class WildfireAPIControllerTests
{
    private Mock<IWildfireRepository> _mockWildfireRepository;
    private Mock<ILogger<WildfireAPIController>> _mockLogger;
    private Mock<INasaService> _mockNasaService;

    private WildfireAPIController _controller;

[SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<WildfireAPIController>>();
        _mockNasaService = new Mock<INasaService>();
        _mockWildfireRepository = new Mock<IWildfireRepository>();

        _controller = new WildfireAPIController(
         
            _mockWildfireRepository.Object,
            _mockLogger.Object,
            _mockNasaService.Object
        );
    }
    [Test]
    public async Task SaveDataToDB_Adds_WildfireData_ToDatabase()
    {
        // Arrange
        var wildfires = new List<FireDTO>
        {
            new FireDTO { Longitude = -120.58M, Latitude = 40.5M, RadiativePower = 12.3M },
            new FireDTO { Longitude = -121.70M, Latitude = 41.3M, RadiativePower = 15.7M }
        };

        _mockNasaService.Setup(service => service.GetFiresAsync()).ReturnsAsync(wildfires);
        _mockWildfireRepository.Setup(repo => repo.AddWildfiresAsync(It.IsAny<List<Fire>>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SaveDataToDB();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        //Assert.That(okResult.Value, Is.EqualTo(new { message = "Wildfire data successfully saved to the database." }));

        // Verify that repository method was called once
        _mockWildfireRepository.Verify(repo => repo.AddWildfiresAsync(It.IsAny<List<Fire>>()), Times.Once);
    }

    [Test]  
    public async Task GetWildfires_Returns_OkResult_With_WildfiresAsync()
    {
        // Arrange
       _mockNasaService.Setup(service => service.GetFiresAsync()).ReturnsAsync(new List<FireDTO>
    {
        new FireDTO { Longitude = -120.58M, Latitude = 40.5M, RadiativePower = 12.3M },
        new FireDTO { Longitude = -121.70M, Latitude = 41.3M, RadiativePower = 15.7M }
    });

        // Act
        var result = await _controller.GetWildfiresAsync();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null); 
        Assert.That(okResult.Value, Is.TypeOf<List<FireDTO>>());  // 
        Assert.That(((List<FireDTO>)okResult.Value).Count, Is.EqualTo(2));
    }

     [Test]
        public async Task ClearWildfiresAsync_RemovesAllWildfires()
        {
            // Arrange
            _mockWildfireRepository.Setup(repo => repo.ClearWildfiresAsync()).Returns(Task.CompletedTask);
            _mockWildfireRepository.Setup(repo => repo.GetWildfireCountAsync()).ReturnsAsync(0);

            // Act
            await _mockWildfireRepository.Object.ClearWildfiresAsync();
            var wildfireCount = await _mockWildfireRepository.Object.GetWildfireCountAsync();

            // Assert
            Assert.That(wildfireCount, Is.EqualTo(0));
        }

        [Test]
        public async Task GetWildfires_Returns_EmptyList_When_NoData()
        {
            // Arrange
            _mockNasaService.Setup(service => service.GetFiresAsync()).ReturnsAsync(new List<FireDTO>());

            // Act
            var result = await _controller.GetWildfiresAsync();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<List<FireDTO>>());
            Assert.That(((List<FireDTO>)okResult.Value).Count, Is.EqualTo(0)); // Should return an empty list
        }

    [Test]  
    public async Task FetchWildfires_Returns_500_If_ApiKey_Missing()
    {
        // Arrange
        _mockNasaService.Setup(service => service.GetFiresAsync()).ReturnsAsync((List<FireDTO>)null);

        // Act
        var result = await _controller.GetWildfiresAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        var statusCodeResult = result as ObjectResult;
        Assert.That(statusCodeResult, Is.Not.Null);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
        Assert.That(statusCodeResult.Value, Is.EqualTo("NASA Service is unavailable or returned no data."));
    }
}*/