using System;
using System.Collections.Generic;
using NUnit.Framework;
using project_wildfire_web.Models;

namespace Project.Wildfire.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_WhenCreated_HasEmptyFiresList()
        {
            // Arrange & Act
            var user = new User();
            
            // Assert
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
    }
}