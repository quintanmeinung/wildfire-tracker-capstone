using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;

namespace project_wildfire_web.Controllers;

public class FireSubscriptionController : Controller
{
    // FireSubscription Constructor

    public class FireSubscription
    {
        //Need this property since fire subs are tied to users
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly IFireRepo
        private readonly IUserRepository _userRepo;
       // private readonly IUserFireSubRepo
    }
}