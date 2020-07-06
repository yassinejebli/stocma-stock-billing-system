using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.OData;
using System.Web.Http.OData.Routing;
using WebApplication1.DATA;

namespace WebApplication1.DATA.OData
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Site>("Sites");
    builder.EntitySet<ArticleSite>("ArticleSites"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SitesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Sites
        [EnableQuery]
        public IQueryable<Site> GetSites()
        {
            return db.Sites;
        }

        // GET: odata/Sites(5)
        [EnableQuery]
        public SingleResult<Site> GetSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.Sites.Where(site => site.Id == key));
        }

        // PUT: odata/Sites(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Site> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            patch.Put(site);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(site);
        }

        // POST: odata/Sites
        public async Task<IHttpActionResult> Post(Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            site.ArticleSites = new List<ArticleSite>();
            db.Sites.Add(site);
            foreach (var article in db.Articles)
            {
                site.ArticleSites.Add(new ArticleSite
                {
                    Article = article,
                    QteStock = 0,
                    Site = site
                });
            }
            await db.SaveChangesAsync();

            return Created(site);
        }

        // PATCH: odata/Sites(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Site> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            patch.Patch(site);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(site);
        }

        // DELETE: odata/Sites(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            //db.Sites.Remove(site);
            site.Disabled = true;
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Sites(5)/ArticleSites
        [EnableQuery]
        public IQueryable<ArticleSite> GetArticleSites([FromODataUri] int key)
        {
            return db.Sites.Where(m => m.Id == key).SelectMany(m => m.ArticleSites);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteExists(int key)
        {
            return db.Sites.Count(e => e.Id == key) > 0;
        }
    }
}
