using project_wildfire_web.Models;

namespace project_wildfire_tests.UnitTests
{
    [TestFixture]
    public class UserLocationTests
    {
        [Test]
        public void InitialUserLocation_HasDefaultProperties()
        {
            // Arrange & Act
            var userLocation = new UserLocation();
            
            // Assert
            Assert.That(userLocation.UserId, Is.Null);
            Assert.That(userLocation.Title, Is.Null);
            Assert.That(userLocation.Latitude, Is.EqualTo(0));
            Assert.That(userLocation.Longitude, Is.EqualTo(0));
        }
    }
}