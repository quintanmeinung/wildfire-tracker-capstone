using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;

namespace project_wildfire_web.Controllers;

[Authorize]
[Route("api/fireSub")]
[ApiController]
public class FireSubscriptionController : ControllerBase
{
    // FireSubscription Constructor
    //Need this property since fire subs are tied to users
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IFireRepo
        private readonly IUserRepository _userRepo;
       // private readonly IUserFireSubRepo
       private readonly IUserFireSubRepository _userFireSubRepo;

       public FireSubscriptionController(UserManager<IdentityUser> userManager,IUserRepository userRepo, IUserFireSubRepository userFireSubRepo )
       {
            _userManager = userManager;
            _userRepo = userRepo;
            _userFireSubRepo = userFireSubRepo;
       }

       [HttpPost("{fireId}")]
       public async Task <IActionResult> Subscribe (int fireId)
       {
            var userId = _userManager.GetUserId(User);
            await _userFireSubRepo.SubscribeAsync(userId,fireId);
            return Ok( new {messge = "Subscribed successfully."});
       }

       [HttpGet]
       public async Task<IActionResult> Unsubscribe(int fireId)
       {    
            var userId = _userManager.GetUserId(User);
            await _userFireSubRepo.UnsubscribeAsync(userId,fireId);
            return Ok( new {messge = "Subscribed successfully."});
       }
       
       [HttpGet("profile/fires")]
          public async Task<IActionResult> GetUserFireSubscriptions()
          {
          var userId = _userManager.GetUserId(User); // or however you get logged-in user
          var fires = await _userFireSubRepo.GetFiresSubsAsync(userId);
          return Ok(fires);
          }
    
}