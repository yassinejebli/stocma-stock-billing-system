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
    La classe WebApiConfig peut exiger d'autres modifications pour ajouter un itinéraire à ce contrôleur. Fusionnez ces instructions dans la méthode Register de la classe WebApiConfig, le cas échéant. Les URL OData sont sensibles à la casse.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication1.DATA;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DgbItem>("DgbItems");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<Dgb>("Dgbs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DgbItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/DgbItems
        [EnableQuery]
        public IQueryable<DgbItem> GetDgbItems()
        {
            return db.DgbItems;
        }

        // GET: odata/DgbItems(5)
        [EnableQuery]
        public SingleResult<DgbItem> GetDgbItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbItems.Where(dgbItem => dgbItem.Id == key));
        }

        // PUT: odata/DgbItems(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<DgbItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbItem dgbItem = await db.DgbItems.FindAsync(key);
            if (dgbItem == null)
            {
                return NotFound();
            }

            patch.Put(dgbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbItem);
        }

        // POST: odata/DgbItems
        public async Task<IHttpActionResult> Post(DgbItem dgbItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DgbItems.Add(dgbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DgbItemExists(dgbItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dgbItem);
        }

        // PATCH: odata/DgbItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<DgbItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbItem dgbItem = await db.DgbItems.FindAsync(key);
            if (dgbItem == null)
            {
                return NotFound();
            }

            patch.Patch(dgbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbItem);
        }

        // DELETE: odata/DgbItems(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            DgbItem dgbItem = await db.DgbItems.FindAsync(key);
            if (dgbItem == null)
            {
                return NotFound();
            }

            db.DgbItems.Remove(dgbItem);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DgbItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/DgbItems(5)/Dgb
        [EnableQuery]
        public SingleResult<Dgb> GetDgb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbItems.Where(m => m.Id == key).Select(m => m.Dgb));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DgbItemExists(Guid key)
        {
            return db.DgbItems.Count(e => e.Id == key) > 0;
        }
    }
}
