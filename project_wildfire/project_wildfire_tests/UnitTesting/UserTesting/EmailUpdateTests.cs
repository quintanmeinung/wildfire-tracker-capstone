/* using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using project_wildfire_web.Areas.Identity.Pages.Account.Manage;
using System.Threading.Tasks;

namespace project_wildfire_tests.AccountPage_Tests
{
    [TestFixture]
    public class EmailUpdateTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private EmailModel _emailModel;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object, 
                Mock.Of<IHttpContextAccessor>(), 
                Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), 
                null, null, null, null);

            _emailModel = new EmailModel(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Test]
        [Category("REQ-1234")] // Requirement ID from ReqRoll
        public async Task SuccessfullyUpdateEmail()
        {
            var user = new IdentityUser { Email = "oldemail@example.com" };
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.SetEmailAsync(user, "newemail@example.com"))
                .ReturnsAsync(IdentityResult.Success);

            _emailModel.Email = "newemail@example.com";

            var result = await _emailModel.OnPostAsync();

            Assert.NotNull(result);
            _userManagerMock.Verify(um => um.SetEmailAsync(user, "newemail@example.com"), Times.Once);
        }

        [Test]
        [Category("REQ-1235")] // Requirement ID from ReqRoll
        public async Task FailToUpdateEmailDueToInvalidFormat()
        {
            var user = new IdentityUser { Email = "oldemail@example.com" };
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.SetEmailAsync(user, "invalid-email"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid email format." }));

            _emailModel.Email = "invalid-email";

            var result = await _emailModel.OnPostAsync();

            Assert.NotNull(result);
            Assert.True(_emailModel.ModelState.ContainsKey(string.Empty));
        }

        [Test]
        [Category("REQ-1236")] // Requirement ID from ReqRoll
        public async Task FailToUpdateEmailWhenUserIsNotFound()
        {
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync((IdentityUser)null);

            var result = await _emailModel.OnPostAsync();

            Assert.IsInstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>(result);
        }
    }
} */