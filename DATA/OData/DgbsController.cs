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
    builder.EntitySet<Dgb>("Dgbs");
    builder.EntitySet<Client>("Clients"); 
    builder.EntitySet<DgbItem>("DgbItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DgbsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Dgbs
        [EnableQuery]
        public IQueryable<Dgb> GetDgbs()
        {
            return db.Dgbs;
        }

        // GET: odata/Dgbs(5)
        [EnableQuery]
        public SingleResult<Dgb> GetDgb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Dgbs.Where(dgb => dgb.Id == key));
        }

        // PUT: odata/Dgbs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Dgb> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dgb dgb = await db.Dgbs.FindAsync(key);
            if (dgb == null)
            {
                return NotFound();
            }

            patch.Put(dgb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgb);
        }

        // POST: odata/Dgbs
        public async Task<IHttpActionResult> Post(Dgb dgb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dgbs.Add(dgb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DgbExists(dgb.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dgb);
        }

        // PATCH: odata/Dgbs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Dgb> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dgb dgb = await db.Dgbs.FindAsync(key);
            if (dgb == null)
            {
                return NotFound();
            }

            patch.Patch(dgb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgb);
        }

        // DELETE: odata/Dgbs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Dgb dgb = await db.Dgbs.FindAsync(key);
            if (dgb == null)
            {
                return NotFound();
            }

            db.Dgbs.Remove(dgb);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Dgbs(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Dgbs.Where(m => m.Id == key).Select(m => m.Client));
        }

        // GET: odata/Dgbs(5)/DgbItems
        [EnableQuery]
        public IQueryable<DgbItem> GetDgbItems([FromODataUri] Guid key)
        {
            return db.Dgbs.Where(m => m.Id == key).SelectMany(m => m.DgbItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DgbExists(Guid key)
        {
            return db.Dgbs.Count(e => e.Id == key) > 0;
        }
    }
}
