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
    builder.EntitySet<FactureFItem>("FactureFItems");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<FactureF>("FactureFs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FactureFItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/FactureFItems
        [EnableQuery]
        public IQueryable<FactureFItem> GetFactureFItems()
        {
            return db.FactureFItems;
        }

        // GET: odata/FactureFItems(5)
        [EnableQuery]
        public SingleResult<FactureFItem> GetFactureFItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFItems.Where(factureFItem => factureFItem.Id == key));
        }

        // PUT: odata/FactureFItems(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<FactureFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureFItem factureFItem = await db.FactureFItems.FindAsync(key);
            if (factureFItem == null)
            {
                return NotFound();
            }

            patch.Put(factureFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(factureFItem);
        }

        // POST: odata/FactureFItems
        public async Task<IHttpActionResult> Post(FactureFItem factureFItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FactureFItems.Add(factureFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FactureFItemExists(factureFItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(factureFItem);
        }

        // PATCH: odata/FactureFItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<FactureFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FactureFItem factureFItem = await db.FactureFItems.FindAsync(key);
            if (factureFItem == null)
            {
                return NotFound();
            }

            patch.Patch(factureFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(factureFItem);
        }

        // DELETE: odata/FactureFItems(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            FactureFItem factureFItem = await db.FactureFItems.FindAsync(key);
            if (factureFItem == null)
            {
                return NotFound();
            }

            db.FactureFItems.Remove(factureFItem);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/FactureFItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/FactureFItems(5)/FactureF
        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FactureFItems.Where(m => m.Id == key).Select(m => m.FactureF));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FactureFItemExists(Guid key)
        {
            return db.FactureFItems.Count(e => e.Id == key) > 0;
        }
    }
}
