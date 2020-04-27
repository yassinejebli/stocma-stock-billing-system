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
    builder.EntitySet<RdbItem>("RdbItems");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<Rdb>("Rdbs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RdbItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/RdbItems
        [EnableQuery]
        public IQueryable<RdbItem> GetRdbItems()
        {
            return db.RdbItems;
        }

        // GET: odata/RdbItems(5)
        [EnableQuery]
        public SingleResult<RdbItem> GetRdbItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(rdbItem => rdbItem.Id == key));
        }

        // PUT: odata/RdbItems(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<RdbItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            patch.Put(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbItem);
        }

        // POST: odata/RdbItems
        public async Task<IHttpActionResult> Post(RdbItem rdbItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbItems.Add(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbItemExists(rdbItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbItem);
        }

        // PATCH: odata/RdbItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<RdbItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            patch.Patch(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbItem);
        }

        // DELETE: odata/RdbItems(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            db.RdbItems.Remove(rdbItem);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/RdbItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/RdbItems(5)/Rdb
        [EnableQuery]
        public SingleResult<Rdb> GetRdb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(m => m.Id == key).Select(m => m.Rdb));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RdbItemExists(Guid key)
        {
            return db.RdbItems.Count(e => e.Id == key) > 0;
        }
    }
}
