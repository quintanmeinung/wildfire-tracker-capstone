using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using project_wildfire_web.Controllers;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Project.Wildfire.Tests.Controllers;

[TestFixture]
public class UserApiControllerTests
{
    private UserApiController _controller;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ILogger<WildfireAPIController>> _loggerMock;
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<WildfireAPIController>>();
        
        // UserManager is abstract, so we need a special setup for it
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        
        #pragma warning disable CS8625 //Suppress warnings for null parameters
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        #pragma warning restore CS8625 //Restore warnings for null parameters
        
        _controller = new UserApiController(
            _loggerMock.Object,
            _userRepositoryMock.Object,
            _userManagerMock.Object);
    }
    
    [Test]
    public async Task SaveModalEdits_WithValidData_ReturnsOk()
    {
        // Arrange
        const string userId = "TBD";
        var profileViewModel = CreateTestProfileViewModel(userId);
        
        var user = new User
        {
            UserId = userId,
            FirstName = "OldFirstName",
            LastName = "OldLastName"
        };
        
        var authUser = new IdentityUser
        {
            Id = userId,
            Email = "old.email@example.com",
            PhoneNumber = "555-987-6543"
        };
        
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync(user);
            
        _userManagerMock.Setup(mgr => mgr.FindByIdAsync(userId))
            .ReturnsAsync(authUser);
            
        _userManagerMock.Setup(mgr => mgr.UpdateAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _controller.SaveModalEdits(profileViewModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
        
        // Verify user properties were updated
        Assert.That(user.FirstName, Is.EqualTo("John"));
        Assert.That(user.LastName, Is.EqualTo("Doe"));
        
        // Verify auth user properties were updated
        Assert.That(authUser.Email, Is.EqualTo("john.doe@example.com"));
        Assert.That(authUser.PhoneNumber, Is.EqualTo("555-123-4567"));
        
        // Verify repo method was called
        _userRepositoryMock.Verify(repo => repo.UpdateUser(user), Times.Once);
        
        // Verify user manager method was called
        _userManagerMock.Verify(mgr => mgr.UpdateAsync(authUser), Times.Once);
    }
    
    [Test]
    public async Task SaveModalEdits_WithInvalidUserId_ReturnsNotFound()
    {
        // Arrange
        const string nonExistentUserId = "nonexistent";
        var profileViewModel = CreateTestProfileViewModel(nonExistentUserId);
        
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(nonExistentUserId))
            .ReturnsAsync(null as User);

        _userManagerMock.Setup(mgr => mgr.FindByIdAsync(nonExistentUserId))
            .ReturnsAsync(new IdentityUser());

        // Act
        var result = await _controller.SaveModalEdits(profileViewModel);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = (NotFoundObjectResult)result;
        Assert.That(notFoundResult.Value, Is.EqualTo("User record not found."));
        VerifyNoUpdatesWerePerformed();
    }

    // Helper methods for better readability and reusability
    private ProfileViewModel CreateTestProfileViewModel(string userId)
    {
        return new ProfileViewModel
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "555-123-4567",
            FireSubscriptions = new List<Fire>()
        };
    }

    private void VerifyNoUpdatesWerePerformed()
    {
        _userRepositoryMock.Verify(
            repo => repo.UpdateUser(It.IsAny<User>()), 
            Times.Never);
            
        _userManagerMock.Verify(
            mgr => mgr.UpdateAsync(It.IsAny<IdentityUser>()), 
            Times.Never);
    }
}
