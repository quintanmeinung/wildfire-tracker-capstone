using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using project_wildfire_web.Controllers;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

[TestFixture] 
public class WildfireAPIControllerTests
{
    private Mock<IWildfireRepository> _mockWildfireRepository;
    private Mock<ILogger<WildfireAPIController>> _mockLogger;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<HttpClient> _mockHttpClient;
    private WildfireAPIController _controller;

   // private Mock<WildfireDbContext> _context;

[SetUp]
    public void Setup()
    {
        _mockWildfireRepository = new Mock<IWildfireRepository>();
        _mockLogger = new Mock<ILogger<WildfireAPIController>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpClient = new Mock<HttpClient>();

        _controller = new WildfireAPIController(
            _mockConfiguration.Object,
            _mockHttpClient.Object,
            _mockLogger.Object,
            _mockWildfireRepository.Object
        );
    }

    [Test]  
    public void GetWildfires_Returns_OkResult_With_Wildfires()
    {
        // Arrange
        var wildfires = new List<Fire>
        {
            new Fire { FireId = 1 },
            new Fire { FireId = 2 }
        };
        _mockWildfireRepository.Setup(repo => repo.GetWildfires()).Returns(wildfires);

        // Act
        var result = _controller.GetWildfires();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null); 
        Assert.That(okResult.Value, Is.TypeOf<List<Fire>>());  // 
        Assert.That(((List<Fire>)okResult.Value).Count, Is.EqualTo(2));
    }

    [Test]  
    public async Task FetchWildfires_Returns_500_If_ApiKey_Missing()
    {
        // Arrange
        _mockConfiguration.Setup(config => config["NASA:FirmsApiKey"]).Returns((string)null);

        // Act
        var result = await _controller.FetchWildfires();

        // Assert
        var statusCodeResult = result as ObjectResult;
        Assert.That(statusCodeResult, Is.Not.Null);
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
    }
}