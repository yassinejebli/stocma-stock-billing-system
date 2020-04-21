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
    builder.EntitySet<DgbF>("DgbFs");
    builder.EntitySet<DgbFItem>("DgbFItems"); 
    builder.EntitySet<Fournisseur>("Fournisseurs"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DgbFsController : ODataController
    {
        private MySaniSoftContext db = new MySaniSoftContext();

        // GET: odata/DgbFs
        [EnableQuery]
        public IQueryable<DgbF> GetDgbFs()
        {
            return db.DgbFs;
        }

        // GET: odata/DgbFs(5)
        [EnableQuery]
        public SingleResult<DgbF> GetDgbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbFs.Where(dgbF => dgbF.Id == key));
        }

        // PUT: odata/DgbFs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<DgbF> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbF dgbF = await db.DgbFs.FindAsync(key);
            if (dgbF == null)
            {
                return NotFound();
            }

            patch.Put(dgbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbF);
        }

        // POST: odata/DgbFs
        public async Task<IHttpActionResult> Post(DgbF dgbF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DgbFs.Add(dgbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DgbFExists(dgbF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(dgbF);
        }

        // PATCH: odata/DgbFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<DgbF> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DgbF dgbF = await db.DgbFs.FindAsync(key);
            if (dgbF == null)
            {
                return NotFound();
            }

            patch.Patch(dgbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DgbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dgbF);
        }

        // DELETE: odata/DgbFs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            DgbF dgbF = await db.DgbFs.FindAsync(key);
            if (dgbF == null)
            {
                return NotFound();
            }

            db.DgbFs.Remove(dgbF);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DgbFs(5)/DgbFItems
        [EnableQuery]
        public IQueryable<DgbFItem> GetDgbFItems([FromODataUri] Guid key)
        {
            return db.DgbFs.Where(m => m.Id == key).SelectMany(m => m.DgbFItems);
        }

        // GET: odata/DgbFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.DgbFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DgbFExists(Guid key)
        {
            return db.DgbFs.Count(e => e.Id == key) > 0;
        }
    }
}
