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
    builder.EntitySet<DgbFItem>("DgbFItems");
    builder.EntitySet<Article>("Articles"); 
    builder.EntitySet<DgbF>("DgbFs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DgbFItemsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/DgbFItems
        [EnableQuery]
        public IQueryable<DgbFItem> GetDgbFItems()
        {
            return db.DgbFItems;
        }

        // GET: odata/DgbFItems(5)
        [EnableQuery]
        public SingleResult<DgbFItem> GetDgbFItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbFItems.Where(dgbFItem => dgbFItem.Id == key));
        }

        // PUT: odata/DgbFItems(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<DgbFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbFItem dgbFItem = await db.DgbFItems.FindAsync(key);
            if (dgbFItem == null)
            {
                return NotFound();
            }

            patch.Put(dgbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbFItem);
        }

        // POST: odata/DgbFItems
        public async Task<IHttpActionResult> Post(DgbFItem dgbFItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DgbFItems.Add(dgbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DgbFItemExists(dgbFItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dgbFItem);
        }

        // PATCH: odata/DgbFItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<DgbFItem> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbFItem dgbFItem = await db.DgbFItems.FindAsync(key);
            if (dgbFItem == null)
            {
                return NotFound();
            }

            patch.Patch(dgbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbFItem);
        }

        // DELETE: odata/DgbFItems(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            DgbFItem dgbFItem = await db.DgbFItems.FindAsync(key);
            if (dgbFItem == null)
            {
                return NotFound();
            }

            db.DgbFItems.Remove(dgbFItem);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DgbFItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbFItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/DgbFItems(5)/DgbF
        [EnableQuery]
        public SingleResult<DgbF> GetDgbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbFItems.Where(m => m.Id == key).Select(m => m.DgbF));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DgbFItemExists(Guid key)
        {
            return db.DgbFItems.Count(e => e.Id == key) > 0;
        }
    }
}
