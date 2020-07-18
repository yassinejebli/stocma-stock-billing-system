using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DATA;
using WebApplication1.Models;

namespace WebApplication1.Auth
{
    public class AuthManager
    {
        public static bool UserHasClaim(string userId, string claim)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var applicationUser = userManager.FindById(userId);
            var userClaim = userManager.GetClaims(userId).FirstOrDefault(x => x.Type == claim);
            var userHasClaim = userClaim != null;

            return userHasClaim;
        }
    }
}