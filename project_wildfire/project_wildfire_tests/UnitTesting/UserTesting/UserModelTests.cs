using project_wildfire_web.Models;

namespace project_wildfire_tests.UnitTests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void InitialUser_HasNullProperties()
        // Except for fires
        {
            // Arrange & Act
            var user = new User();
            
            // Assert
            Assert.That(user.UserId, Is.Null);
            Assert.That(user.FirstName, Is.Null);
            Assert.That(user.LastName, Is.Null);
            Assert.That(user.Fires, Is.Not.Null);
            Assert.That(user.Fires, Is.Empty);
            
        }

        [Test]
        public void UserProperties_WhenSet_CanBeRetrieved()
        {
            // Arrange
            var user = new User();
            
            // Act
            user.UserId = "user123";
            user.FirstName = "John";
            user.LastName = "Doe";
            
            // Assert
            Assert.That(user.UserId, Is.EqualTo("user123"));
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
        }

        [Test]
        public void Fires_WhenAddingItem_UpdatesCollection()
        {
            // Arrange
            var user = new User();
            var fire = new Fire { FireId = 1 };
            
            // Act
            user.Fires.Add(fire);
            
            // Assert
            Assert.That(user.Fires, Contains.Item(fire));
            Assert.That(user.Fires.Count, Is.EqualTo(1));
        }

    }
}