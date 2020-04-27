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
using System.Web.OData;
using System.Web.Http.OData.Routing;
using WebApplication1.DATA;

namespace WebApplication1.DATA.OData
{
    /*
    La classe WebApiConfig peut exiger d'autres modifications pour ajouter un itinéraire à ce contrôleur. Fusionnez ces instructions dans la méthode Register de la classe WebApiConfig, le cas échéant. Les URL OData sont sensibles à la casse.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<RdbFItem>("RdbFItems");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<RdbF>("RdbFs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RdbFItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/RdbFItems
        [EnableQuery]
        public IQueryable<RdbFItem> GetRdbFItems()
        {
            return db.RdbFItems;
        }

        // GET: odata/RdbFItems(5)
        [EnableQuery]
        public SingleResult<RdbFItem> GetRdbFItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(rdbFItem => rdbFItem.Id == key));
        }

        // PUT: odata/RdbFItems(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<RdbFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            patch.Put(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbFItem);
        }

        // POST: odata/RdbFItems
        public async Task<IHttpActionResult> Post(RdbFItem rdbFItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbFItems.Add(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbFItemExists(rdbFItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbFItem);
        }

        // PATCH: odata/RdbFItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<RdbFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            patch.Patch(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbFItem);
        }

        // DELETE: odata/RdbFItems(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            db.RdbFItems.Remove(rdbFItem);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/RdbFItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/RdbFItems(5)/RdbF
        [EnableQuery]
        public SingleResult<RdbF> GetRdbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(m => m.Id == key).Select(m => m.RdbF));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RdbFItemExists(Guid key)
        {
            return db.RdbFItems.Count(e => e.Id == key) > 0;
        }
    }
}
