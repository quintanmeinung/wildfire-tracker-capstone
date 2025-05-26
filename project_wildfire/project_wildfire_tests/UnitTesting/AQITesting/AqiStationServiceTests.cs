using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using project_wildfire_web.Models;     // for AqiStation
using project_wildfire_web.Services;   // for AqiStationService
//dotnet test --filter FullyQualifiedName~GetAllAqiStations_ReturnsAllSeededStations

namespace project_wildfire_web.Tests.Services
{
    [TestFixture]
    public class AqiStationServiceTests
    {
        private FireDataDbContext _context;
        private AqiStationService _service;

        [SetUp]
        public void SetUp()
        {
            // Create a new in-memory DB for each test
            var options = new DbContextOptionsBuilder<FireDataDbContext>()
                .UseInMemoryDatabase(databaseName: "AqiStationTestDb")
                .Options;

            _context = new FireDataDbContext(options);

            // Seed data — only uses properties defined in my actual model
            _context.AqiStations.AddRange(
                new AqiStation { StationId = "1", Name = "Station A" },
                new AqiStation { StationId = "2", Name = "Station B" }
            );
            _context.SaveChanges();

            _service = new AqiStationService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetAllAqiStations_ReturnsAllSeededStations()
        {
            // Act
            List<AqiStation> result = _service.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(s => s.Name == "Station A"));
            Assert.That(result.Any(s => s.Name == "Station B"));
        }
    }
}
