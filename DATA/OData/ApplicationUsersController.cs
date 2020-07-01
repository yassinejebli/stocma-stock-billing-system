using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.OData;
using WebApplication1.Models;

namespace WebApplication1.DATA.OData
{
    //[Authorize(Roles = "Admin")]
    public class ApplicationUsersController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [EnableQuery]
        public IQueryable<ApplicationUser> GetApplicationUsers()
        {
            return db.GetApplicationUsers().ToList().AsQueryable();
        }

        [EnableQuery]
        public SingleResult<ApplicationUser> GetApplicationUser([FromODataUri] string key)
        {
            return SingleResult.Create<ApplicationUser>(this.db.GetApplicationUsers().Where<ApplicationUser>((Expression<Func<ApplicationUser, bool>>)(applicationUser => applicationUser.Id == key)));
        }

        public IHttpActionResult Put([FromODataUri] string key, string UserName)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var applicationUser = userManager.FindById(key);
            
           // if (applicationUser == null)
             //   return (IHttpActionResult)this.NotFound();

            applicationUser.UserName = UserName;

            userManager.Update(applicationUser);

            try
            {
                int num = this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ApplicationUserExists(new Guid(key)))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<ApplicationUser>(applicationUser);
        }

        public async Task<IHttpActionResult> Post(ApplicationUser applicationUser)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            this.db.GetApplicationUsers().Add(applicationUser);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.ApplicationUserExists(new Guid(applicationUser.Id)))
                    return (IHttpActionResult)this.Conflict();
                throw;
            }
            return (IHttpActionResult)this.Created<ApplicationUser>(applicationUser);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<ApplicationUser> patch)
        {
            if (!this.ModelState.IsValid)
                return (IHttpActionResult)this.BadRequest(this.ModelState);
            ApplicationUser applicationUser = this.db.GetApplicationUsers().Find(key);
            if (applicationUser == null)
                return (IHttpActionResult)this.NotFound();
            patch.Patch(applicationUser);
            try
            {
                int num = this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ApplicationUserExists(key))
                    return (IHttpActionResult)this.NotFound();
                throw;
            }
            return (IHttpActionResult)this.Updated<ApplicationUser>(applicationUser);
        }

        public IHttpActionResult Delete([FromODataUri] string key)
        {
            ApplicationUser async = this.db.GetApplicationUsers().Find((object)key);
            if (async == null)
                return (IHttpActionResult)this.NotFound();
            this.db.GetApplicationUsers().Remove(async);
            int num = this.db.SaveChanges();
            return (IHttpActionResult)this.StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(Guid key)
        {
            return this.db.GetApplicationUsers().Count<ApplicationUser>((Expression<Func<ApplicationUser, bool>>)(e => e.Id == key.ToString())) > 0;
        }
    }
}
