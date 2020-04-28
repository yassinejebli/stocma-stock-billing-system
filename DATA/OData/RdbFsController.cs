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
    builder.EntitySet<RdbF>("RdbFs");
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    builder.EntitySet<RdbFItem>("RdbFItems"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RdbFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/RdbFs
        [EnableQuery]
        public IQueryable<RdbF> GetRdbFs()
        {
            return db.RdbFs;
        }

        // GET: odata/RdbFs(5)
        [EnableQuery]
        public SingleResult<RdbF> GetRdbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFs.Where(rdbF => rdbF.Id == key));
        }

        // PUT: odata/RdbFs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<RdbF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            patch.Put(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbF);
        }

        // POST: odata/RdbFs
        public async Task<IHttpActionResult> Post(RdbF rdbF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbFs.Add(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbFExists(rdbF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbF);
        }

        // PATCH: odata/RdbFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<RdbF> patch)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            patch.Patch(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbF);
        }

        // DELETE: odata/RdbFs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            db.RdbFs.Remove(rdbF);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/RdbFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/RdbFs(5)/RdbFItems
        [EnableQuery]
        public IQueryable<RdbFItem> GetRdbFItems([FromODataUri] Guid key)
        {
            return db.RdbFs.Where(m => m.Id == key).SelectMany(m => m.RdbFItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RdbFExists(Guid key)
        {
            return db.RdbFs.Count(e => e.Id == key) > 0;
        }
    }
}
