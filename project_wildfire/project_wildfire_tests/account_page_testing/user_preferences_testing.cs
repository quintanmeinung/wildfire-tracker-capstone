/*
// Entire file content commented out
using System;
using System.Collections.Generic;
using NUnit.Framework;
using project_wildfire_web.Models;

namespace Project.Wildfire.Tests.Models
{
    [TestFixture]
    public class UserPreferencesTests
    {
        [Test]
        public void InitialUserPreferences_HasDefaultProperties()
        {
            // Arrange & Act
            var userPreferences = new UserPreferences();
            
            // Assert
            Assert.That(userPreferences.UserId, Is.Null);
            Assert.That(userPreferences.FontSize, Is.EqualTo("medium"));
            Assert.That(userPreferences.ContrastMode, Is.False);
            Assert.That(userPreferences.TextToSpeech, Is.False);
        }

        [Test]
        public void UserPreferences_WhenChanged_CanBeRetrieved()
        {
            // Arrange
            var userPreferences = new UserPreferences();
            
            // Act
            userPreferences.UserId = "user123";
            userPreferences.FontSize = "large";
            userPreferences.ContrastMode = true;
            userPreferences.TextToSpeech = true;
            
            // Assert
            Assert.That(userPreferences.UserId, Is.EqualTo("user123"));
            Assert.That(userPreferences.FontSize, Is.EqualTo("large"));
            Assert.That(userPreferences.ContrastMode, Is.EqualTo(true));
            Assert.That(userPreferences.TextToSpeech, Is.EqualTo(true));
        }

    }
}
*/