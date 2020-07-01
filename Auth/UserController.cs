using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
            var userClaim = userManager.GetClaims(userId).FirstOrDefault(x=>x.Type == claim);
            var userHasClaim = userClaim != null;
            if (enabled&&!userHasClaim)
                userManager.AddClaim(userId, new Claim(claim, "true"));
            else if(userHasClaim)
                userManager.RemoveClaim(userId, userClaim);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}