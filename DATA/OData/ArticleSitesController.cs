﻿using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.OData.Routing;

namespace WebApplication1.DATA.OData
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ArticleSite>("ArticleSites");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<Site>("Sites"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [Authorize]
    public class ArticleSitesController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/ArticleSites
        [EnableQuery]
        public IQueryable<ArticleSite> GetArticleSites()
        {
            return db.ArticleSites;
        }

        // GET: odata/ArticleSites(5)
        [EnableQuery]
        public SingleResult<ArticleSite> GetArticleSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(articleSite => articleSite.IdSite == key));
        }

        // PUT: odata/ArticleSites(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ArticleSite> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArticleSite articleSite = await db.ArticleSites.FindAsync(key);
            if (articleSite == null)
            {
                return NotFound();
            }

            patch.Put(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleSiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(articleSite);
        }

        // POST: odata/ArticleSites
        public async Task<IHttpActionResult> Post(ArticleSite articleSite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ArticleSites.Add(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticleSiteExists(articleSite.IdSite))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(articleSite);
        }

        // PATCH: odata/ArticleSites(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ArticleSite> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArticleSite articleSite = await db.ArticleSites.FindAsync(key);
            if (articleSite == null)
            {
                return NotFound();
            }

            patch.Patch(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleSiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(articleSite);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int keyIdSite, [FromODataUri] Guid keyIdArticle)
        {
            ArticleSite articleSite = await db.ArticleSites.FirstOrDefaultAsync(x => x.IdSite == keyIdSite && x.IdArticle == keyIdArticle);
            if (articleSite == null)
            {
                return NotFound();
            }

            //db.ArticleSites.Remove(articleSite);
            articleSite.Disabled = true;
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ArticleSites(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(m => m.IdSite == key).Select(m => m.Article));
        }

        // GET: odata/ArticleSites(5)/Site
        [EnableQuery]
        public SingleResult<Site> GetSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(m => m.IdSite == key).Select(m => m.Site));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleSiteExists(int key)
        {
            return db.ArticleSites.Count(e => e.IdSite == key) > 0;
        }
    }
}
