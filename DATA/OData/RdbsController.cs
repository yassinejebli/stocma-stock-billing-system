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
using System.Web.Http.OData;
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
    builder.EntitySet<Rdb>("Rdbs");
    builder.EntitySet<Client>("Clients"); 
    builder.EntitySet<RdbItem>("RdbItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RdbsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/Rdbs
        [EnableQuery]
        public IQueryable<Rdb> GetRdbs()
        {
            return db.Rdbs;
        }

        // GET: odata/Rdbs(5)
        [EnableQuery]
        public SingleResult<Rdb> GetRdb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Rdbs.Where(rdb => rdb.Id == key));
        }

        // PUT: odata/Rdbs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Rdb> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            patch.Put(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdb);
        }

        // POST: odata/Rdbs
        public async Task<IHttpActionResult> Post(Rdb rdb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rdbs.Add(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbExists(rdb.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdb);
        }

        // PATCH: odata/Rdbs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Rdb> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            patch.Patch(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdb);
        }

        // DELETE: odata/Rdbs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            db.Rdbs.Remove(rdb);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Rdbs(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Rdbs.Where(m => m.Id == key).Select(m => m.Client));
        }

        // GET: odata/Rdbs(5)/RdbItems
        [EnableQuery]
        public IQueryable<RdbItem> GetRdbItems([FromODataUri] Guid key)
        {
            return db.Rdbs.Where(m => m.Id == key).SelectMany(m => m.RdbItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RdbExists(Guid key)
        {
            return db.Rdbs.Count(e => e.Id == key) > 0;
        }
    }
}
