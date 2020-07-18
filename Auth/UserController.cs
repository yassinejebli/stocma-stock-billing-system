using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Auth
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPut]
        public ActionResult UpdateUser(ApplicationUser user)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var applicationUser = userManager.FindById(user.Id);

            applicationUser.UserName = user.UserName;
            applicationUser.Email = user.UserName;

            userManager.Update(applicationUser);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult UpdatePassword(string userId, string newPassword)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            userManager.RemovePassword(userId);
            userManager.AddPassword(userId, newPassword);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult SetClaim(string userId, string claim, bool enabled)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var applicationUser = userManager.FindById(userId);
            var userClaim = userManager.GetClaims(userId).FirstOrDefault(x => x.Type == claim);
            var userHasClaim = userClaim != null;
            if (enabled && !userHasClaim)
                userManager.AddClaim(userId, new Claim(claim, "true"));
            else if (userHasClaim)
                userManager.RemoveClaim(userId, userClaim);

            userHasClaim = userManager.GetClaims(userId).FirstOrDefault(x => x.Type == claim) != null;

            userManager.UpdateSecurityStamp(userId);

            return Json(new { userHasClaim }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HasClaim(string userId, string claim)
        {
            return Json(new { userHasClaim = AuthManager.UserHasClaim(userId, claim) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateUser(string username, string password)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = new ApplicationUser();
            user.Id = Guid.NewGuid().ToString();
            user.Email = username;
            user.UserName = username;
            var result = userManager.Create(user, password);


            if (result.Succeeded)
                return Json(new { user }, JsonRequestBehavior.AllowGet);
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpDelete]
        public ActionResult RemoveUser(string userId)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(userId);
            userManager.Delete(user);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult GetCurrentUserClaims()
        {
            var userId = User.Identity.GetUserId();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(userId);

            var claims = user.Claims.ToList();
            var isAdmin = User.IsInRole("Admin");
            var username = User.Identity.Name;

            return Json(new { username, isAdmin, claims = claims.Select(x => x.ClaimType) }, JsonRequestBehavior.AllowGet);
        }
    }
}