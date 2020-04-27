using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
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
    builder.EntitySet<FakeFactureFItem>("FakeFactureFItems");
    builder.EntitySet<ArticleFacture>("ArticleFactures"); 
    builder.EntitySet<FakeFactureF>("FakeFacturesF"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FakeFactureFItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FakeFactureFItems
        [EnableQuery]
        public IQueryable<FakeFactureFItem> GetFakeFactureFItems()
        {
            return db.FakeFactureFItems;
        }

        // GET: odata/FakeFactureFItems(5)
        [EnableQuery]
        public SingleResult<FakeFactureFItem> GetFakeFactureFItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFactureFItems.Where(fakeFactureFItem => fakeFactureFItem.Id == key));
        }

        // PUT: odata/FakeFactureFItems(5)
        public IHttpActionResult Put([FromODataUri] Guid key, Delta<FakeFactureFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureFItem fakeFactureFItem = db.FakeFactureFItems.Find(key);
            if (fakeFactureFItem == null)
            {
                return NotFound();
            }

            patch.Put(fakeFactureFItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fakeFactureFItem);
        }

        // POST: odata/FakeFactureFItems
        public IHttpActionResult Post(FakeFactureFItem fakeFactureFItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FakeFactureFItems.Add(fakeFactureFItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FakeFactureFItemExists(fakeFactureFItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(fakeFactureFItem);
        }

        // PATCH: odata/FakeFactureFItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] Guid key, Delta<FakeFactureFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureFItem fakeFactureFItem = db.FakeFactureFItems.Find(key);
            if (fakeFactureFItem == null)
            {
                return NotFound();
            }

            patch.Patch(fakeFactureFItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fakeFactureFItem);
        }

        // DELETE: odata/FakeFactureFItems(5)
        public IHttpActionResult Delete([FromODataUri] Guid key)
        {
            FakeFactureFItem fakeFactureFItem = db.FakeFactureFItems.Find(key);
            if (fakeFactureFItem == null)
            {
                return NotFound();
            }

            db.FakeFactureFItems.Remove(fakeFactureFItem);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FakeFactureFItems(5)/ArticleFacture
        [EnableQuery]
        public SingleResult<ArticleFacture> GetArticleFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFactureFItems.Where(m => m.Id == key).Select(m => m.ArticleFacture));
        }

        // GET: odata/FakeFactureFItems(5)/FakeFactureF
        [EnableQuery]
        public SingleResult<FakeFactureF> GetFakeFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFactureFItems.Where(m => m.Id == key).Select(m => m.FakeFactureF));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FakeFactureFItemExists(Guid key)
        {
            return db.FakeFactureFItems.Count(e => e.Id == key) > 0;
        }
    }
}
